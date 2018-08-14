using UnityEngine;

[System.Serializable]
public class Range
{
	public float min;
	public float max;

	Range(float min, float max)
	{
		this.min = min;
		this.max = max;
	}
}
