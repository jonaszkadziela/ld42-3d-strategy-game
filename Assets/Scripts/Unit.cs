using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
	public NavMeshAgent agent;

	private Camera mainCamera;
	private Selectable selectable;

	void Start()
	{
		mainCamera = Camera.main;
		selectable = GetComponentInChildren<Selectable>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && selectable.isSelected)
		{
			RaycastHit hit;

			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
			{
				if (hit.collider.gameObject.tag != "Unit")
				{
					agent.SetDestination(hit.point);
				}
			}
		}
	}
}
