using UnityEngine;

public class SavesManager
{
	public static void Save()
	{
		if (ScoreManager.MostUnits > ScoreManager.HighScore)
		{
			PlayerPrefs.SetInt("HighScore", ScoreManager.MostUnits);
		}
		if (!PlayerPrefs.HasKey("HighScore"))
		{
			PlayerPrefs.SetInt("HighScore", 0);
		}
	}

	public static void Load()
	{
		ScoreManager.HighScore = PlayerPrefs.GetInt("HighScore", 0);
	}
}
