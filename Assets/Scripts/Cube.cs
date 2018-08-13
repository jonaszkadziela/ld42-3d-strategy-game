using UnityEngine;
using UnityEngine.AI;

public class Cube : MonoBehaviour
{
	public LayerMask unitMask;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	public bool SelfDestruct()
	{
		Vector3 overlapBoxScale = this.transform.localScale / 2;
		overlapBoxScale.y *= 2;

		Collider[] units = Physics.OverlapBox(this.transform.position, overlapBoxScale, Quaternion.identity, unitMask);

		foreach (Collider unit in units)
		{
			unit.transform.parent.GetComponent<Unit>().enabled = false;
		}

		ScoreManager.ChangeCurrentUnits(-units.Length);

		rb.isKinematic = false;
		this.gameObject.layer = 0;

		return true;
	}
}
