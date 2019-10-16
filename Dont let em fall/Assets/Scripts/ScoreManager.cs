using UnityEngine;

public class ScoreManager
{
	public static float TimeLeft;
	public static int MostUnits;
	public static int CurrentUnits;
	public static int HighScore;

	public static float DefaultTimeLeft = 180f;
	public static int DefaultMostUnits = 0;
	public static int DefaultCurrentUnits = 0;
	public static int DefaultHighScore = 0;

	public static void InitializeVariables()
	{
		TimeLeft = DefaultTimeLeft;
		MostUnits = DefaultMostUnits;
		CurrentUnits = DefaultCurrentUnits;
		HighScore = DefaultHighScore;
	}

	public static void ChangeCurrentUnits(int amount)
	{
		CurrentUnits += amount;
		if (MostUnits < CurrentUnits)
		{
			MostUnits = CurrentUnits;
		}
	}
}
