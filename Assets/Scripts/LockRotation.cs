using UnityEngine;

public class LockRotation : MonoBehaviour
{
	private Quaternion initialRotation;

	void Start()
	{
		initialRotation = this.transform.rotation;
	}

	void LateUpdate()
	{
		this.transform.rotation = initialRotation;
	}
}
