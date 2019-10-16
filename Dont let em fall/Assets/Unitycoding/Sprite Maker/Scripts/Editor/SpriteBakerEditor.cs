using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using System;

public class SpriteBakerEditor : EditorWindow
{
	public static SpriteBakerEditor ShowWindow ()
	{
		SpriteBakerEditor window = EditorWindow.GetWindow<SpriteBakerEditor> ("Sprite Baker");
		window.minSize = new Vector2 (288, 100);
		return window;
	}

	[SerializeField]
	private Vector2 scrollPosition;
	[SerializeField]
	private Vector2 resolution = new Vector2 (1024, 1024);
	[SerializeField]
	private Camera camera;

	[SerializeField]
	private int frames = 10;
	[SerializeField]
	private UnityEngine.Object target;
	[SerializeField]
	private List<AnimationClip> clips = new List<AnimationClip> ();
	private ReorderableList clipList;
	[SerializeField]
	private int currentFrame;
	private Editor editor;
	private Action bakeAction;
	private int clipIndex = 0;
	private List<Texture2D> textures = new List<Texture2D> ();
	private ReorderableList viewList;
	[SerializeField]
	private List<Vector2> views = new List<Vector2> ();
	private float particleDuration;

	private SerializedObject serializedObject;
	private SerializedProperty mResolution;
	private SerializedProperty mCamera;
	private SerializedProperty mFrames;
	private SerializedProperty mTarget;

	private void OnEnable ()
	{
		serializedObject = new SerializedObject (this);
		mResolution = serializedObject.FindProperty ("resolution");
		mCamera = serializedObject.FindProperty ("camera");
		mFrames = serializedObject.FindProperty ("frames");

		mTarget = serializedObject.FindProperty ("target");

		clipList = new ReorderableList (serializedObject, serializedObject.FindProperty ("clips"), true, true, true, true);
		clipList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			var element = clipList.serializedProperty.GetArrayElementAtIndex (index);
			rect.y += 2;
			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField (rect, element, GUIContent.none);
		};
		clipList.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField (rect, "Animation Clips");
		};
		//
		viewList = new ReorderableList (serializedObject, serializedObject.FindProperty ("views"), true, true, true, true);
		viewList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			var element = viewList.serializedProperty.GetArrayElementAtIndex (index);
			rect.y += 2;
			rect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField (rect, element, GUIContent.none);
		};
		viewList.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField (rect, "Views");
		};
		LoadSettings ();
	}

	private void OnDestroy ()
	{
		SaveSettings ();
	}

	private void OnGUI ()
	{
		GUILayout.Space (3f);
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, EditorStyles.inspectorDefaultMargins);
		serializedObject.Update ();
		EditorGUIUtility.labelWidth = 90f;

		EditorGUILayout.PropertyField (mCamera);

		EditorGUILayout.PropertyField (mResolution);
		mResolution.vector2Value = new Vector2 (Mathf.Clamp (mResolution.vector2Value.x, 32, 8192), Mathf.Clamp (mResolution.vector2Value.y, 32, 8192));

		GUILayout.Space (3.0f);
		viewList.DoLayoutList ();

		EditorGUILayout.PropertyField (mFrames);
		mFrames.intValue = Mathf.Clamp (mFrames.intValue, 2, int.MaxValue);

		EditorGUILayout.PropertyField (mTarget);
	
		if (target is Animator) {
			GUILayout.Space (3.0f);
			clipList.DoLayoutList ();
		} else if (target is ParticleSystem) {
			particleDuration = EditorGUILayout.FloatField ("Duration", particleDuration);
		}
	
		GUILayout.Space (1);
		bool isEnabled = GUI.enabled;
		GUI.enabled = target != null;
		if (GUILayout.Button ("Bake")) {
			textures.Clear ();
			//clips[clipIndex].SampleAnimation ((target as Animator).gameObject, 0f);

			bakeAction = StartBake;
		}
		GUI.enabled = isEnabled;

		GUILayout.Space (1);
		if (camera != null && camera.targetTexture != null) {
			if (editor == null) {
				editor = Editor.CreateEditor (camera.targetTexture);
			}
			Rect rect = GUILayoutUtility.GetLastRect ();
			rect.y += 34;
			rect.x = 0;
			rect.width = Screen.width;
			rect.height = Screen.height - 24 - rect.y;

			editor.OnInteractivePreviewGUI (rect, (GUIStyle)"AnimationKeyframeBackground");
			Rect rect1 = rect;
			rect1.y -= 18;
			GUI.Label (rect1, "", EditorStyles.toolbar);
			GUI.Label (rect1, "Preview");
		
			if (target != null) {
				rect1.x += 50;
				rect1.width -= 50;
				rect1.height = 18;
				int mFrame = EditorGUI.IntSlider (rect1, currentFrame, 0, frames);
				if (mFrame != currentFrame) {
					Selection.activeObject = target;
				}
				currentFrame = mFrame;
				float ratio = (float)(currentFrame) / (float)(frames);
			
				GUI.Label (new Rect (Screen.width - 50, Screen.height - 40, 50, 20), (ratio * 100f).ToString ("f1") + "%");
			}
		}
		serializedObject.ApplyModifiedProperties ();
		GUILayout.EndScrollView ();
	}

	private void Simulate ()
	{
		if (target != null) {
			float ratio = (float)(currentFrame) / (float)(frames);

			float time = ratio;
			if ((target is Animator) && clips.Count > clipIndex && clips [clipIndex] != null) {
				time *= clips [clipIndex].length;
				clips [clipIndex].SampleAnimation ((target as Animator).gameObject, time);
			} else if (target is ParticleSystem) {
				ParticleSystem particleSystem = (target as ParticleSystem);
				time *= particleDuration;
				particleSystem.Simulate (time, true);

			}
		}
		Repaint ();
	
	}

	private void SaveSettings ()
	{
		EditorPrefs.SetInt ("ResolutionX", (int)resolution.x);
		EditorPrefs.SetInt ("ResolutionY", (int)resolution.y);
		EditorPrefs.SetInt ("Frames", frames);
	}

	private void LoadSettings ()
	{
		resolution = new Vector2 (EditorPrefs.GetInt ("ResolutionX", 512), EditorPrefs.GetInt ("ResolutionY", 512));
		frames = EditorPrefs.GetInt ("Frames", 10);
	}

	#region Bake

	private void Update ()
	{

		if (bakeAction != null) {
			bakeAction.Invoke ();
		}
		Simulate ();
		if (target != null && (target is GameObject)) {
			UnityEngine.Object mTarget = (target as GameObject).GetComponent<Animator> ();
			if (mTarget == null) {
				mTarget = (target as GameObject).GetComponent<ParticleSystem> ();
				if (mTarget != null) {
					particleDuration = (mTarget as ParticleSystem).main.duration;
				}
			}
			if (mTarget != null) {
				target = mTarget;
			}
			EditorUtility.SetDirty (this);
		}

		if (target == null) {
			target = FindObjectOfType<Animator> ();
			if (target == null) {
				target = FindObjectOfType<ParticleSystem> ();
			}
			EditorUtility.SetDirty (this);
		}
		if (camera == null) {
			camera = FindObjectOfType<Camera> ();
			camera.clearFlags = CameraClearFlags.SolidColor;
			EditorUtility.SetDirty (this);
		} else if (camera.targetTexture == null) {
			RenderTexture rt = new RenderTexture (1024, 1024, 24);
			rt.hideFlags = HideFlags.HideAndDontSave;
			camera.targetTexture = rt;
		} 

		AdjustView ();
	}

	private void AdjustView ()
	{
		if (views.Count > viewIndex && camera != null && target != null && currentView != views [viewIndex]) {
			Transform tr = (target as Animator).GetComponent<Transform> ();
			
			Vector3 euler = camera.transform.eulerAngles;
			euler.y = views [viewIndex].y;
			euler.x = views [viewIndex].x;
			camera.transform.eulerAngles = euler;
			currentView = views [viewIndex];
			camera.transform.position = tr.position;
			camera.transform.position -= tr.position;
			camera.transform.position -= camera.transform.forward * 5;
		}
	}

	[SerializeField]
	private Vector2 currentView;
	private int viewIndex;

	private void StartBake ()
	{
		clips.RemoveAll (x => x == null);
		currentFrame = 0;
		//Simulate ();
		bakeAction = Bake;
	}

	private void Bake ()
	{
		Color cameraColor = camera.backgroundColor;
		RenderTexture prev = camera.targetTexture;
		
		float ratio = (float)currentFrame / (float)(frames);
		EditorUtility.DisplayProgressBar ("Progress...", ((target is Animator) && clips.Count > 0 ? clips [clipIndex].name : target.name) + "  " + currentFrame + "/" + frames, ratio);
		//Simulate ();

		RenderTexture rt = new RenderTexture ((int)resolution.x, (int)resolution.y, 24, RenderTextureFormat.ARGB32);
		camera.backgroundColor = Color.black;
		camera.targetTexture = rt;
		camera.Render ();
		RenderTexture.active = rt;

		Texture2D black = new Texture2D ((int)resolution.x, (int)resolution.y, TextureFormat.RGB24, false);
		black.ReadPixels (new Rect (0, 0, (int)resolution.x, (int)resolution.y), 0, 0);
		black.Apply ();

		camera.backgroundColor = Color.white;
		camera.Render ();
		Texture2D white = new Texture2D ((int)resolution.x, (int)resolution.y, TextureFormat.RGB24, false);
		white.ReadPixels (new Rect (0, 0, (int)resolution.x, (int)resolution.y), 0, 0);
		white.Apply ();
		
		camera.targetTexture = null;
		RenderTexture.active = null; 

		Texture2D tex = ExtractTexture (black, white, (int)resolution.x, (int)resolution.y);
		Vector2 view = views.Count > 0 ? views [viewIndex] : Vector2.zero;

		tex.name = System.String.Format (((target is Animator) ? (clips.Count > clipIndex ? clips [clipIndex].name : "sprite") : target.name) + " " + view.ToString () + " {0:D04}", currentFrame);
		tex.alphaIsTransparency = true;
		textures.Add (tex);

		camera.targetTexture = prev;
		camera.backgroundColor = cameraColor;
		currentFrame++;

		if (currentFrame > frames - 1) {
			if (clips.Count - 1 > clipIndex) {
				clipIndex++;
				bakeAction = StartBake;
			} else {
				bakeAction = EndBake;
			}
		}
	
	}

	private void EndBake ()
	{
		//textures.RemoveAt (textures.Count - 1);
		if (views.Count - 1 > viewIndex) {
			viewIndex++;
			clipIndex = 0;
			currentFrame = 0;
			bakeAction = StartBake;
			return;
		}
		bakeAction = null;
		clipIndex = 0;
		currentFrame = 0;
		viewIndex = 0;
		EditorUtility.ClearProgressBar ();
		SpritePackerEditor packer = FindObjectOfType<SpritePackerEditor> ();
		if (packer == null) {
			packer = SpritePackerEditor.ShowWindow ();
		}
		packer.textures = textures;
		packer.Repaint ();
	}

	private  Texture2D ExtractTexture (Texture2D black, Texture2D white, int width, int height)
	{
		Texture2D tex = new Texture2D (width, height, TextureFormat.ARGB32, false);
		for (int y = 0; y < tex.height; y++) {
			for (int x = 0; x < tex.width; x++) {
				Color pixelOnBlack = black.GetPixel (x, y);
				Color pixelOnWhite = white.GetPixel (x, y);
				float alpha = 1.0f - (pixelOnWhite.r - pixelOnBlack.r); 
				Color color = Color.clear;
				if (alpha != 0)
					color = pixelOnBlack / alpha;
				color.a = alpha;
				tex.SetPixel (x, y, color);
			}
		}
		tex.Apply ();
		return tex;
	}

	#endregion

	public enum PivotPosition
	{
		Center,
		CenterOfMass,
		TopPixel,
		RightPixel,
		LeftPixel,
		BottomPixel,
	}
}
