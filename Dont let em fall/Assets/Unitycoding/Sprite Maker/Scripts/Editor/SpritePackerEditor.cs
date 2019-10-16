using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SpritePackerEditor : EditorWindow
{
	[SerializeField]
	public List<Texture2D> textures = new List<Texture2D> ();
	[SerializeField]
	private Vector2 scroll;
	private Rect content = new Rect ();
	private int mMaxSize = 1024;
	private PackMode packMode;
	private Alignment alignment = Alignment.Compact;
	private int rows = 1;
	private int columns = 1;
	private int padding = 2;
	private bool trimSingle;
	private int paddingSingle;
	private Pivot pivot;
	private Vector2 pivotPosition;
	private bool saveSingleSprites;

	private Dictionary<Pivot,Vector2> pivotMap = new Dictionary<Pivot, Vector2> () {
		{ Pivot.Center,new Vector2 (0.5f, 0.5f) },
		{ Pivot.TopLeft, new Vector2 (0f, 1f) },
		{ Pivot.Top, new Vector2 (0.5f, 1f) },
		{ Pivot.TopRight, new Vector2 (1f, 1f) },
		{ Pivot.Left, new Vector2 (0f, 0.5f) },
		{ Pivot.Right, new Vector2 (1f, 0.5f) },
		{ Pivot.BottomLeft, new Vector2 (0f, 0f) },
		{ Pivot.Bottom, new Vector2 (0.5f, 0f) },
		{ Pivot.BottomRight, new Vector2 (1f, 0f) },
	};

	public static SpritePackerEditor ShowWindow ()
	{
		SpritePackerEditor window = EditorWindow.GetWindow<SpritePackerEditor> ("Sprite Packer");
		window.minSize = new Vector2 (167f, 215f);
		return window;
	}

	private void OnGUI ()
	{
		textures.RemoveAll (x => x == null);
		if (textures.Count == 0 || textures [textures.Count - 1] != null) {
			textures.Add (null);
		}
		DoSettingsGUI ();

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Clear", GUILayout.Width (80))) {
			textures.Clear ();
		}
		bool enabled = GUI.enabled;
		GUI.enabled = textures.Count > 1;
		if (GUILayout.Button ("Pack", GUILayout.Width (80))) {
			textures.RemoveAll (x => x == null);
			if (trimSingle) {
				textures = TextureUtility.TrimSymetric (textures.ToArray (), paddingSingle).ToList ();
				/*for(int i=0;i< textures.Count;i++){
					Texture2D tex=textures[i].Trim(paddingSingle);
					tex.name=textures[i].name;
					tex.alphaIsTransparency=true;
					textures[i]=tex;
				}*/
			}
			Rect[] rects = null;
			Texture2D atlas = new Texture2D (mMaxSize, mMaxSize, TextureFormat.RGBA32, false);
			atlas.Clear ();
			switch (packMode) {
			case PackMode.Automatic:
				rects = SpritePackerUtility.AutoPack (ref atlas, textures.ToArray (), padding, mMaxSize);
				break;
			case PackMode.Grid:
				switch (alignment) {
				case Alignment.Compact:
					rects = SpritePackerUtility.GridPack (ref atlas, textures.ToArray (), padding);
					break;
				case Alignment.Row:
					rects = SpritePackerUtility.GridPack (ref atlas, textures.ToArray (), padding, Mathf.CeilToInt ((float)textures.Count / rows), rows);
					break;
				case Alignment.Column:
					rects = SpritePackerUtility.GridPack (ref atlas, textures.ToArray (), padding, columns, Mathf.CeilToInt ((float)textures.Count / columns));
					break;
				}


				break;
			}
			string path = EditorUtility.SaveFilePanelInProject ("Save texture as PNG",
				              "Atlas.png",
				              "png",
				              "Please enter a file name to save the texture to");
			if (!string.IsNullOrEmpty (path)) {
				SaveAtlas (atlas, rects, pivot, path);
				if (saveSingleSprites) {
					string file = System.IO.Path.GetFileName (path);
					float cnt = 0f;
					foreach (Texture2D tex in textures) {
						tex.SaveTexture (path.Replace (file, tex.name + ".png"));
						float progress = cnt / (float)textures.Count;
						EditorUtility.DisplayProgressBar ("Progress...", tex.name, progress);
						cnt++;
					}
					EditorUtility.ClearProgressBar ();
				}
			}
		}
		GUI.enabled = enabled;
		GUILayout.EndHorizontal ();
		GUILayout.Label (textures.Count - 1 + " Textures");
		DrawTextures ();
		AcceptTextureDrag ();
	}

	private Texture2D SaveAtlas (Texture2D atlas, Rect[] rects, Pivot pivot, string path)
	{
		byte[] bytes = atlas.EncodeToPNG (); 
		System.IO.File.WriteAllBytes (path, bytes);
		if (AssetDatabase.LoadAssetAtPath (path, typeof(Texture2D)) == null) {
			AssetDatabase.Refresh ();
		}
		TextureImporter tempImporter = (TextureImporter)AssetImporter.GetAtPath (path);
		if (tempImporter != null) {
			tempImporter.textureType = TextureImporterType.Sprite;
			tempImporter.spriteImportMode = SpriteImportMode.Multiple;
			tempImporter.maxTextureSize = mMaxSize;
			
			int count = textures.Count;
			SpriteMetaData[] meta = new SpriteMetaData[count];

			for (int i = 0; i < count; i++) {
				meta [i].name = textures [i].name.ToString ();
				meta [i].alignment = (int)SpriteAlignment.Custom;
				Rect rect = rects [i];
				if (pivot == Pivot.CenterOfMass) {
					meta [i].pivot = CalcCenterOfMass (atlas, rect);
				} else {
					meta [i].pivot = pivotPosition;
				}
				meta [i].rect = rect;
			}
			
			tempImporter.spritesheet = meta;
			AssetDatabase.ImportAsset (path);
			AssetDatabase.Refresh ();
		}
		return (Texture2D)AssetDatabase.LoadAssetAtPath (path, typeof(Texture2D));
	}

	public static Vector2 CalcCenterOfMass (Texture2D texture, Rect rect)
	{
		float i = 0f;
		float total = 0f;
		Vector2 centerOfMass = Vector2.zero;
		
		for (int y = (int)rect.y; y < (int)(rect.height + rect.y); y++) {
			for (int x = (int)rect.x; x < (int)(rect.width + rect.x); x++) {
				Color pixel = texture.GetPixel (x, y);
				i = (pixel.a > 0f ? 1f : 0f);
				centerOfMass.x += i * (x - rect.x);
				centerOfMass.y += i * (y - rect.y);
				total += i;
			}
		}
		centerOfMass.x /= (total * rect.width);
		centerOfMass.y /= (total * rect.height);
		
		return centerOfMass;
	}

	private void DoSettingsGUI ()
	{
		EditorGUIUtility.labelWidth = 80;
		packMode = (PackMode)EditorGUILayout.EnumPopup ("Pack Mode", packMode);
		if (packMode == PackMode.Grid) {
			alignment = (Alignment)EditorGUILayout.EnumPopup ("Alignment", alignment);
			switch (alignment) {
			case Alignment.Row:
				rows = EditorGUILayout.IntField ("Rows", rows);
				break;
			case Alignment.Column:
				columns = EditorGUILayout.IntField ("Columns", columns);
				break;
			}
		}
		mMaxSize = EditorGUILayout.IntField ("Max Size", mMaxSize);
		trimSingle = EditorGUILayout.Toggle ("Trim", trimSingle);
		if (trimSingle) {
			EditorGUI.indentLevel += 1;
			paddingSingle = EditorGUILayout.IntField ("Padding", paddingSingle);
			EditorGUI.indentLevel -= 1;
		}
		saveSingleSprites = EditorGUILayout.Toggle ("Save Single", saveSingleSprites);

		padding = EditorGUILayout.IntField ("Atlas Padding", padding);
		pivot = (Pivot)EditorGUILayout.EnumPopup ("Pivot", pivot);
		
		if (pivot != Pivot.CenterOfMass) {
			bool enabled = GUI.enabled;
			GUI.enabled = pivot == Pivot.Custom;
			if (pivot != Pivot.Custom) {
				pivotPosition = pivotMap [pivot];
			}
			pivotPosition = EditorGUILayout.Vector2Field ("Custom Pivot", pivotPosition);
			GUI.enabled = enabled;
		}
	}

	private void DrawTextures ()
	{
		Rect mPosition = GUILayoutUtility.GetRect (0, 0);
		scroll = GUI.BeginScrollView (new Rect (mPosition.x, mPosition.y, position.width, position.height - mPosition.y), scroll, content, false, false);
		Rect rect = new Rect (2, 2, 80, 80);
		for (int i = 0; i < textures.Count; i++) {
			textures [i] = (Texture2D)EditorGUI.ObjectField (rect, textures [i], typeof(Texture2D), false);
			rect.x += 82;
			if (rect.x + 82 > this.position.width) {
				rect.y += 82;
				rect.x = 2;
			}	
		}
		content.height = rect.y + 80;
		GUI.EndScrollView ();
	}

	private void AcceptTextureDrag ()
	{
		
		EventType eventType = Event.current.type;
		bool isAccepted = false;
		if ((eventType == EventType.DragUpdated || eventType == EventType.DragPerform)) {
			DragAndDrop.visualMode = DragAndDropVisualMode.Link;
			
			if (eventType == EventType.DragPerform) {
				DragAndDrop.AcceptDrag ();
				isAccepted = true;
			}
			Event.current.Use ();
		}
		
		if (isAccepted) {
			Object[] objs = DragAndDrop.objectReferences;
			objs = objs.OrderBy (x => x.name).ToArray ();
			for (int i = 0; i < objs.Length; i++) {
				if (objs [i].GetType () == typeof(Texture2D)) {
					if (!textures.Contains (objs [i] as Texture2D)) {
						textures.Add (objs [i] as Texture2D);
					}
				}
			}
		}
	}

	public enum PackMode
	{
		Automatic,
		Grid
	}

	public enum Pivot
	{
		Center,
		TopLeft,
		Top,
		TopRight,
		Left,
		Right,
		BottomLeft,
		Bottom,
		BottomRight,
		CenterOfMass,
		Custom,
		
	}

	public enum Alignment
	{
		Compact,
		Row,
		Column
	}
}
