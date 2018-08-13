using UnityEngine;

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
	}

	public void ToggleGameOver()
	{
		GameOver = true;

		Camera.main.GetComponent<CameraController>().MoveCameraToTarget(CameraController.InitialPosition, 0.05f);

		topUI.SetActive(false);
		gameOverUI.SetActive(true);
	}

	public void RestartGame()
	{
		SceneFade.Instance.FadeTo(SceneFade.ActiveSceneName);
	}
}
