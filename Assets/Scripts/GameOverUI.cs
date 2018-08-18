using System.Collections;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
	public GameObject gameOverUI;

	public TextMeshProUGUI savedUnitsText;
	public TextMeshProUGUI highScoreText;

	public float animationsDelay = 1f;
	public float textCounterDelay = 0.1f;

	void Update()
	{
		if (GameManager.GameOver)
		{
			if (!gameOverUI.activeSelf)
			{
				TriggerGameOver();
			}
			return;
		}
	}

	void TriggerGameOver()
	{
		gameOverUI.SetActive(true);

		savedUnitsText.text = "0";
		highScoreText.text = "0";

		Invoke("StartAnimations", animationsDelay);
	}

	public void PlayAgain()
	{
		gameObject.SetActive(false);
		GameManager.Instance.RestartGame();
	}

	public void Menu()
	{
		gameObject.SetActive(false);
		SceneFade.Instance.FadeTo(SceneFade.MainMenuSceneName);
	}

	private void StartAnimations()
	{
		StartCoroutine(AnimatedCounter(savedUnitsText, ScoreManager.MostUnits));
		StartCoroutine(AnimatedCounter(highScoreText, ScoreManager.HighScore));
	}

	private IEnumerator AnimatedCounter(TextMeshProUGUI text, int targetNumber)
	{
		int number = 0;

		while (number < targetNumber)
		{
			number++;
			text.text = number.ToString();
			yield return new WaitForSeconds(textCounterDelay);
		}
	}
}
