using UnityEngine;

public class Selectable : MonoBehaviour
{
	public GameObject selectionIndicatior;
	[HideInInspector]
	public bool isSelected = false;

	public void Toggle()
	{
		isSelected = !isSelected;
		selectionIndicatior.SetActive(isSelected);
	}
}
