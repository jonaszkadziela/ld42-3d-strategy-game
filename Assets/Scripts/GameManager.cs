using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public static bool GameOver;

	public GameObject topUI;
	public GameObject gameOverUI;

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
		GameOver = false;
		SavesManager.Load();
	}

	public void ToggleGameOver()
	{
		GameOver = true;
		SavesManager.Save();
		UpdateAnalytics();

		Camera.main.GetComponent<CameraController>().MoveCameraToTarget(CameraController.InitialPosition, 0.05f);

		topUI.SetActive(false);
		gameOverUI.SetActive(true);
	}

	public void UpdateAnalytics()
	{
		Analytics.CustomEvent("PlayerScore", new Dictionary<string, object>
			{
				{ "HighScore", ScoreManager.HighScore }
			});
	}

	public void RestartGame()
	{
		SceneFade.Instance.FadeTo(SceneFade.GameSceneName);
	}
}
