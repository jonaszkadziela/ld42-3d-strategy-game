using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
	public static SceneFade Instance;
	public static string GameSceneName = "Main";
	public static string MainMenuSceneName = "Main Menu";
	public static string ActiveSceneName;

	public float fadeDuration = 1f;
	public Image overlay;
	public AnimationCurve curve;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		ActiveSceneName = SceneManager.GetActiveScene().name;
		StartCoroutine(FadeIn());
	}

	public void FadeTo(string sceneName)
	{
		StartCoroutine(FadeOut(sceneName));
	}

	private IEnumerator FadeIn()
	{
		float time = fadeDuration;

		while (time > 0f)
		{
			time -= Time.deltaTime;
			float alhpa = curve.Evaluate(time);
			overlay.color = new Color(0f, 0f, 0f, alhpa);
			yield return 0;
		}
	}

	private IEnumerator FadeOut(string sceneName)
	{
		float time = 0f;

		while (time < fadeDuration)
		{
			time += Time.deltaTime;
			float alhpa = curve.Evaluate(time);
			overlay.color = new Color(0f, 0f, 0f, alhpa);
			yield return 0;
		}

		SceneManager.LoadScene(sceneName);
	}
}
