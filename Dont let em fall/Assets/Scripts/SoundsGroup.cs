using UnityEngine;

[System.Serializable]
public class SoundsGroup
{
	public string name;
	[Range(0f, 1f)]
	public float pitchVariationRange = 0f;
	[HideInInspector]
	public AudioSource source;
	public Sound[] sounds;
}
