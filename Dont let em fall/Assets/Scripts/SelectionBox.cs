using UnityEngine;

public class SelectionBox : MonoBehaviour
{
	public RectTransform selectionBoxImage;

	private GameObject selectionBox;
	private Camera mainCamera;
	private Vector3 startPos;
	private Vector3 endPos;

	void Start()
	{
		mainCamera = Camera.main;
		selectionBox = selectionBoxImage.gameObject;
		selectionBox.SetActive(false);
	}

	void Update()
	{
		if (GameManager.GameOver || SelectionManager.DisableSelection)
		{
			if (selectionBox.activeSelf)
			{
				selectionBox.SetActive(false);
			}
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
			{
				startPos = mainCamera.WorldToScreenPoint(hit.point);
				selectionBox.SetActive(true);
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			selectionBox.SetActive(false);
		}
		if (Input.GetMouseButton(0))
		{
			endPos = Input.mousePosition;

			Vector3 center = (startPos + endPos) / 2;

			float sizeX = Mathf.Abs(startPos.x - endPos.x);
			float sizeY = Mathf.Abs(startPos.y - endPos.y);

			selectionBoxImage.position = center;
			selectionBoxImage.sizeDelta = new Vector2(sizeX, sizeY);
		}
	}
}
