using UnityEngine;

public class LockRotation : MonoBehaviour
{
	private Quaternion initialRotation;

	void Start()
	{
		initialRotation = transform.rotation;
	}

	void LateUpdate()
	{
		transform.rotation = initialRotation;
	}
}
