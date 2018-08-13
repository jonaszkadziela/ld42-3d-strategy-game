using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static float TimeLeft;
	public static int MostUnits;
	public static int CurrentUnits;
	public static int HighScore;

	public float startTimeLeft = 60f;
	public int startMostUnits = 0;
	public int startCurrentUnits = 0;

	void Start()
	{
		TimeLeft = startTimeLeft;
		MostUnits = startMostUnits;
		CurrentUnits = startCurrentUnits;
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
