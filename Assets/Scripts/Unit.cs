using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
	public NavMeshAgent agent;

	private Camera mainCamera;
	private Selectable selectable;
	private SelectionManager sm;

	void Start()
	{
		mainCamera = Camera.main;
		selectable = GetComponentInChildren<Selectable>();
		sm = GameManager.Instance.GetComponent<SelectionManager>();
		sm.selectableItems.Add(selectable);
	}

	void Update()
	{
		if (GameManager.GameOver)
		{
			return;
		}
		if (Input.GetMouseButtonDown(1) && selectable.isSelected)
		{
			RaycastHit hit;

			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
			{
				if (hit.collider.gameObject.tag != "Unit" && agent.isOnNavMesh)
				{
					agent.SetDestination(hit.point);
				}
			}
		}
	}

	public void OnDisable()
	{
		sm.Deselect(selectable);
		sm.selectableItems.Remove(selectable);

		GameObject child = selectable.gameObject;

		child.GetComponent<NavMeshAgent>().enabled = false;
		child.GetComponent<Rigidbody>().isKinematic = false;
		child.layer = 0;

		selectable.enabled = false;
		enabled = false;
	}
}
