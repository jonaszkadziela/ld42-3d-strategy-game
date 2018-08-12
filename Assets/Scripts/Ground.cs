using UnityEngine;

public class Ground : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Unit")
		{
			Destroy(other.transform.parent.gameObject);
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
