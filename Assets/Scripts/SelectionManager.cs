using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
	public LayerMask selectablesLayer;

	private Camera mainCamera;
	private List<Selectable> selectedItems;

	[HideInInspector]
	public bool isSelecting = false;
	public List<Selectable> selectableItems;
	private Vector3 startMousePos;
	private Vector3 endMousePos;

	void Awake()
	{
		mainCamera = Camera.main;
		selectedItems = new List<Selectable>();
		selectableItems = new List<Selectable>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			startMousePos = mainCamera.ScreenToViewportPoint(Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, selectablesLayer))
			{
				Selectable selectable = hit.collider.GetComponent<Selectable>();

				if (Input.GetButton("Group Selection"))
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
			else
			{
				if (!Input.GetButton("Group Selection"))
				{
					DeselectAll();
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			endMousePos = mainCamera.ScreenToViewportPoint(Input.mousePosition);

			if (startMousePos != endMousePos)
			{
				BoxSelectObjects();
			}
		}
	}

	public void Deselect(Selectable s)
	{
		if (s.isSelected)
		{
			s.Toggle();
		}
	}

	public void DeselectAll()
	{
		if (selectedItems.Count > 0)
		{
			foreach (Selectable s in selectedItems)
			{
				Deselect(s);
			}
			selectedItems.Clear();
		}
	}

	private void BoxSelectObjects()
	{
		List<Selectable> removeItems = new List<Selectable>();

		if (!Input.GetButton("Group Selection"))
		{
			DeselectAll();
		}

		Rect boxSelection = new Rect(startMousePos.x, startMousePos.y, endMousePos.x - startMousePos.x, endMousePos.y - startMousePos.y);

		foreach (Selectable s in selectableItems)
		{
			if (s != null)
			{
				if (boxSelection.Contains(mainCamera.WorldToViewportPoint(s.gameObject.transform.position), true) && !s.isSelected)
				{
					selectedItems.Add(s);
					s.Toggle();
				}
			}
			else
			{
				removeItems.Add(s);
			}
		}

		if (removeItems.Count > 0)
		{
			foreach (Selectable s in removeItems)
			{
				selectableItems.Remove(s);
			}
			removeItems.Clear();
		}
	}
}
