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
			PlayerPrefs.SetInt("HighScore", ScoreManager.DefaultHighScore);
		}

		PlayerPrefs.SetFloat("MasterVolume", GameSettings.MasterVolume);
		PlayerPrefs.SetFloat("MusicVolume", GameSettings.MusicVolume);
		PlayerPrefs.SetFloat("SoundEffectsVolume", GameSettings.SoundEffectsVolume);
		PlayerPrefs.SetFloat("PanSpeed", GameSettings.PanSpeed);
		PlayerPrefs.SetFloat("ScrollSpeed", GameSettings.ScrollSpeed);

		int gameOverSoundEffect = GameSettings.GameOverSoundEffect ? 1 : 0;
		PlayerPrefs.SetInt("GameOverSoundEffect", gameOverSoundEffect);

		PlayerPrefs.Save();
	}

	public static void Load()
	{
		ScoreManager.HighScore = PlayerPrefs.GetInt("HighScore", ScoreManager.DefaultHighScore);

		GameSettings.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", GameSettings.DefaultMasterVolume);
		GameSettings.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", GameSettings.DefaultMusicVolume);
		GameSettings.SoundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", GameSettings.DefaultSoundEffectsVolume);
		GameSettings.PanSpeed = PlayerPrefs.GetFloat("PanSpeed", GameSettings.DefaultPanSpeed);
		GameSettings.ScrollSpeed = PlayerPrefs.GetFloat("ScrollSpeed", GameSettings.DefaultScrollSpeed);

		int defaultGameOverSoundEffect = GameSettings.DefaultGameOverSoundEffect ? 1 : 0;

		int gameOverSoundEffect = PlayerPrefs.GetInt("GameOverSoundEffect", defaultGameOverSoundEffect);
		GameSettings.GameOverSoundEffect = (gameOverSoundEffect >= 1) ? true : false;
	}
}
