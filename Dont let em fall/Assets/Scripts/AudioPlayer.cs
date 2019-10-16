using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
	public AudioSource audioSource;
	public SoundsGroup[] soundGroups;

	void Awake()
	{
		foreach (SoundsGroup s in soundGroups)
		{
			s.source = audioSource;
		}
	}

	public void Play(string name)
	{
		SoundsGroup sg = Array.Find(soundGroups, s => s.name == name);

		if (sg == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		Sound randomSound = AudioManager.GetRandomSound(sg);

		float pitch = randomSound.pitch + UnityEngine.Random.Range(-sg.pitchVariationRange, sg.pitchVariationRange);

		sg.source.clip = randomSound.clip;
		sg.source.volume = randomSound.volume;
		sg.source.pitch = pitch;
		sg.source.loop = randomSound.loop;

		sg.source.Play();
	}
}
