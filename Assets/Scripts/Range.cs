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

	public static Range operator*(Range r, int i)
	{
		r.min *= i;
		r.max *= i;

		return r;
	}
}
