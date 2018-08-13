using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
	public TextMeshProUGUI mostUnitsText;
	public TextMeshProUGUI currentUnitsText;
	public TextMeshProUGUI timeLeftText;

	void Update()
	{
		mostUnitsText.text = ScoreManager.MostUnits.ToString();
		currentUnitsText.text = ScoreManager.CurrentUnits.ToString();
		timeLeftText.text = ((int)ScoreManager.TimeLeft).ToString();
	}
}
