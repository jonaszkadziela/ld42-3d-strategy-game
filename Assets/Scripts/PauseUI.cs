using UnityEngine;

public class PauseUI : MonoBehaviour
{
	public GameObject pauseUI;

	void Update()
	{
		if (GameManager.GameOver)
		{
			if (pauseUI.activeSelf)
			{
				SetPause(false);
			}
		}
		else
		{
			if (Input.GetButtonDown("Pause"))
			{
				SetPause(!pauseUI.activeSelf);
			}
		}
	}

	public void SetPause(bool isPaused)
	{
		pauseUI.SetActive(isPaused);

		if (pauseUI.activeSelf)
		{
			Time.timeScale = 0f;
			SelectionManager.DisableSelection = true;
		}
		else
		{
			Time.timeScale = 1f;
			SelectionManager.DisableSelection = false;
		}
	}

	public void Restart()
	{
		SetPause(false);
		SceneFade.Instance.FadeTo(SceneFade.GameSceneName);
	}

	public void Menu()
	{
		SetPause(false);
		SceneFade.Instance.FadeTo(SceneFade.MainMenuSceneName);
	}
}
