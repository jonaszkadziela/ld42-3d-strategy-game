using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public static MapGenerator Map;
	public static bool GameOver;

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

		Map = GameObject.FindWithTag("MapGenerator").GetComponent<MapGenerator>();
	}

	void Start()
	{
		GameOver = false;
		SavesManager.Load();
	}

	public void TriggerGameOver()
	{
		GameOver = true;
		SavesManager.Save();
		UpdateAnalytics();

		Camera.main.GetComponent<CameraController>().MoveCameraToTarget(CameraController.InitialPosition, 0.05f);
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
