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

	public Image overlay;
	public AnimationCurve curve;
	public float fadeDuration = 2f;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		ActiveSceneName = SceneManager.GetActiveScene().name;
	}

	void Start()
	{
		switch (ActiveSceneName)
		{
			case "Main Menu":
				AudioManager.Instance.Play("MainMenu");
			break;

			case "Main":
				AudioManager.Instance.Play("BGM");
			break;
		}

		AudioManager.Instance.Unmute();
		StartCoroutine(FadeIn());
	}

	public void FadeTo(string sceneName)
	{
		AudioManager.Instance.Mute();
		StartCoroutine(FadeOut(sceneName));
	}

	private IEnumerator FadeIn()
	{
		float time = fadeDuration;

		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;

			float alpha = curve.Evaluate(time / fadeDuration);
			overlay.color = new Color(0f, 0f, 0f, alpha);

			yield return 0;
		}
	}

	private IEnumerator FadeOut(string sceneName)
	{
		float time = 0f;

		while (time < fadeDuration)
		{
			time += Time.unscaledDeltaTime;

			float alpha = curve.Evaluate(time / fadeDuration);
			overlay.color = new Color(0f, 0f, 0f, alpha);

			yield return 0;
		}

		SceneManager.LoadScene(sceneName);
	}
}
