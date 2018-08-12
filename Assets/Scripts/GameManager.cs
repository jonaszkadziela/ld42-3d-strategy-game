using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public bool gameOver = false;

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

	public void GameOver()
	{
		gameOver = true;
	}
}
