using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
	public GameObject mainMenuUI;
	public GameObject settingsUI;

	public void Play()
	{
		SceneFade.Instance.FadeTo(SceneFade.GameSceneName);
	}

	public void Settings()
	{
		mainMenuUI.SetActive(false);
		settingsUI.SetActive(true);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
