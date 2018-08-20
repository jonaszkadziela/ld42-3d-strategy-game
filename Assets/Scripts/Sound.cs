using UnityEngine;

[System.Serializable]
public class Sound
{
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0.1f, 2f)]
	public float pitch = 1f;

	public bool loop = false;
}
