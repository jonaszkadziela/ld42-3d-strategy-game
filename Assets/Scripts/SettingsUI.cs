using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
	public GameObject parentUI;
	public GameObject settingsUI;
	public Transform pagesContainer;

	public Button backButton;
	public Button nextButton;

	public Slider masterSlider;
	public Slider musicSlider;
	public Slider sfxSlider;
	public Slider cameraSpeedSlider;
	public Slider zoomSpeedSlider;
	public Toggle gameOverSFX;

	private int pageIndex = 0;

	void Start()
	{
		SavesManager.Load();
		InitializeValues();
		DetermineInteractableButtons();
	}

	public void SetMasterVolume(float value)
	{
		GameSettings.MasterVolume = value;
		AudioManager.Instance.AdjustMixerVolumes();
	}

	public void SetMusicVolume(float value)
	{
		GameSettings.MusicVolume = value;
		AudioManager.Instance.AdjustMixerVolumes();
	}

	public void SetSoundEffectsVolume(float value)
	{
		GameSettings.SoundEffectsVolume = value;
		AudioManager.Instance.AdjustMixerVolumes();
	}

	public void SetPanSpeed(float value)
	{
		GameSettings.PanSpeed = value;
	}

	public void SetScrollSpeed(float value)
	{
		GameSettings.ScrollSpeed = value;
	}

	public void SetGameOverSoundEffect(bool value)
	{
		GameSettings.GameOverSoundEffect = value;
	}

	public void ResetHighScore()
	{
		ScoreManager.HighScore = 0;
		PlayerPrefs.SetInt("HighScore", ScoreManager.DefaultHighScore);
	}

	public void Back()
	{
		pagesContainer.GetChild(pageIndex).gameObject.SetActive(false);
		pagesContainer.GetChild(--pageIndex).gameObject.SetActive(true);
		DetermineInteractableButtons();
	}

	public void Next()
	{
		pagesContainer.GetChild(pageIndex).gameObject.SetActive(false);
		pagesContainer.GetChild(++pageIndex).gameObject.SetActive(true);
		DetermineInteractableButtons();
	}

	public void Exit()
	{
		SavesManager.Save();
		settingsUI.SetActive(false);
		parentUI.SetActive(true);
	}

	private void InitializeValues()
	{
		masterSlider.value = GameSettings.MasterVolume;
		musicSlider.value = GameSettings.MusicVolume;
		sfxSlider.value = GameSettings.SoundEffectsVolume;
		cameraSpeedSlider.value = GameSettings.PanSpeed;
		zoomSpeedSlider.value = GameSettings.ScrollSpeed;
		gameOverSFX.isOn = GameSettings.GameOverSoundEffect;
	}

	private void DetermineInteractableButtons()
	{
		if (pageIndex == 0)
		{
			backButton.interactable = false;
		}
		else
		{
			backButton.interactable = true;
		}
		if (pageIndex == (pagesContainer.transform.childCount - 1))
		{
			nextButton.interactable = false;
		}
		else
		{
			nextButton.interactable = true;
		}
	}
}
