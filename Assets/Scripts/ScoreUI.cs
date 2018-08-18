using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
	public GameObject scoreUI;

	public TextMeshProUGUI mostUnitsText;
	public TextMeshProUGUI currentUnitsText;
	public TextMeshProUGUI timeLeftText;

	void Update()
	{
		if (GameManager.GameOver)
		{
			if (scoreUI.activeSelf)
			{
				scoreUI.SetActive(false);
			}
			return;
		}
		if (Input.GetButtonDown("Toggle UI"))
		{
			scoreUI.SetActive(!scoreUI.activeSelf);
		}
		mostUnitsText.text = ScoreManager.MostUnits.ToString();
		currentUnitsText.text = ScoreManager.CurrentUnits.ToString();
		timeLeftText.text = ((int)ScoreManager.TimeLeft).ToString();
	}
}
