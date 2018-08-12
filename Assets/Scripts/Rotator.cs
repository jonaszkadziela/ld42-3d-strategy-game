using UnityEngine;

public class Rotator : MonoBehaviour
{
	[Range(0, 1)]
	public int xRotation;
	[Range(0, 1)]
	public int yRotation;
	[Range(0, 1)]
	public int zRotation;
	public float rotationSpeed;

	void Update()
	{
		this.transform.Rotate(new Vector3(xRotation, yRotation, zRotation) * rotationSpeed * Time.deltaTime, Space.World);
	}
}
