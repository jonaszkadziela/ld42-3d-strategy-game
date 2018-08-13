using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
	public void Play()
	{
		SceneFade.Instance.FadeTo(SceneFade.GameSceneName);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
