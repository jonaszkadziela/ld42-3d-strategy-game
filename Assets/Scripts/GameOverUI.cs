using System.Collections;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
	public TextMeshProUGUI savedUnitsText;
	public TextMeshProUGUI highScoreText;

	public float animationsDelay = 1f;
	public float textCounterDelay = 0.1f;

	void OnEnable()
	{
		savedUnitsText.text = "0";
		highScoreText.text = "0";

		Invoke("StartAnimations", animationsDelay);
	}

	public void PlayAgain()
	{
		GameManager.Instance.RestartGame();
	}

	private void StartAnimations()
	{
		StartCoroutine(AnimatedCounter(savedUnitsText, ScoreManager.MostUnits));
		StartCoroutine(AnimatedCounter(highScoreText, ScoreManager.MostUnits));
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
