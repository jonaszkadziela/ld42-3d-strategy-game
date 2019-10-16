using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;
	public AudioMixer audioMixer;
	public AudioMixerGroup musicGroup;
	public AudioMixerGroup soundEffectsGroup;

	public string musicContainerName = "Music";
	public string soundEffectsContainerName = "Sound Effects";

	public float fadeDuration = 2f;
	public AnimationCurve fadeCurve;

	public SoundsGroup[] music;
	public SoundsGroup[] soundEffects;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		GameObject musicContainer = new GameObject(musicContainerName);
		musicContainer.transform.parent = transform;

		GameObject soundEffectsContainer = new GameObject(soundEffectsContainerName);
		soundEffectsContainer.transform.parent = transform;

		AudioSource musicSource = musicContainer.AddComponent<AudioSource>();

		foreach (SoundsGroup m in music)
		{
			m.source = musicSource;
			m.source.outputAudioMixerGroup = musicGroup;
		}

		foreach (SoundsGroup s in soundEffects)
		{
			s.source = soundEffectsContainer.AddComponent<AudioSource>();
			s.source.outputAudioMixerGroup = soundEffectsGroup;
		}
	}

	void Start()
	{
		AdjustMixerVolumes();
		audioMixer.SetFloat("MasterVolume", RemapVolume(0f));
	}

	public void Play(string name)
	{
		SoundsGroup sg = Array.Find(music, m => m.name == name);

		if (sg == null)
		{
			sg = Array.Find(soundEffects, s => s.name == name);
		}
		if (sg == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		Sound randomSound = GetRandomSound(sg);

		float pitch = randomSound.pitch + UnityEngine.Random.Range(-sg.pitchVariationRange, sg.pitchVariationRange);

		sg.source.clip = randomSound.clip;
		sg.source.volume = randomSound.volume;
		sg.source.pitch = pitch;
		sg.source.loop = randomSound.loop;

		sg.source.Play();
	}

	public void Mute()
	{
		StartCoroutine(FadeOut());
	}

	public void Unmute()
	{
		StartCoroutine(FadeIn());
	}

	public static Sound GetRandomSound(SoundsGroup sg)
	{
		if (sg.sounds.Length > 0)
		{
			int randomIndex = UnityEngine.Random.Range(0, sg.sounds.Length);
			return sg.sounds[randomIndex];
		}

		return null;
	}

	public static float RemapVolume(float value01)
	{
		return Mathf.Lerp(-80f, 0f, value01);
	}

	public void AdjustMixerVolumes()
	{
		audioMixer.SetFloat("MasterVolume", RemapVolume(GameSettings.MasterVolume));
		audioMixer.SetFloat("MusicVolume", RemapVolume(GameSettings.MusicVolume));
		audioMixer.SetFloat("SFXVolume", RemapVolume(GameSettings.SoundEffectsVolume));
		audioMixer.SetFloat("ImportantSFXVolume", RemapVolume(GameSettings.SoundEffectsVolume));
	}

	private IEnumerator FadeIn()
	{
		float time = 0f;

		while (time < fadeDuration)
		{
			time += Time.unscaledDeltaTime;
			float volume = fadeCurve.Evaluate(time / fadeDuration);

			audioMixer.SetFloat("MasterVolume", RemapVolume(Mathf.Clamp(volume, 0f, GameSettings.MasterVolume)));

			yield return 0;
		}
	}

	private IEnumerator FadeOut()
	{
		float time = fadeDuration;

		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			float volume = fadeCurve.Evaluate(time / fadeDuration);

			audioMixer.SetFloat("MasterVolume", RemapVolume(Mathf.Clamp(volume, 0f, GameSettings.MasterVolume)));

			yield return 0;
		}
	}
}
