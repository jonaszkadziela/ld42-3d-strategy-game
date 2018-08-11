using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
	public LayerMask selectablesLayer;

	private Camera mainCamera;
	private List<Selectable> selectedItems;

	void Start()
	{
		mainCamera = Camera.main;
		selectedItems = new List<Selectable>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, selectablesLayer))
			{
				Selectable selectable = hit.collider.GetComponent<Selectable>();

				if (Input.GetButton("Group selection"))
				{
					if (selectable.isSelected)
					{
						selectedItems.Remove(selectable);
					}
					else
					{
						selectedItems.Add(selectable);
					}
					selectable.Toggle();
				}
				else
				{
					if (selectedItems.Count == 1 && selectedItems.Contains(selectable))
					{
						selectedItems.Remove(selectable);
					}
					else
					{
						DeselectAll();
						selectedItems.Add(selectable);
					}
					selectable.Toggle();
				}
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			DeselectAll();
		}
	}

	private void DeselectAll()
	{
		if (selectedItems.Count > 0)
		{
			foreach (Selectable s in selectedItems)
			{
				if (s.isSelected)
				{
					s.Toggle();
				}
			}
			selectedItems.Clear();
		}
	}
}
