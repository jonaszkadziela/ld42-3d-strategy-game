using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
	[HideInInspector]
	public AudioPlayer audioPlayer;
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

		audioPlayer = GetComponent<AudioPlayer>();
		audioPlayer.Play("UnitSpawn");
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
				if (agent.isOnNavMesh)
				{
					agent.SetDestination(hit.point);
				}
			}
		}
	}

	public void OnDisable()
	{
		selectable.GetComponent<NavMeshAgent>().enabled = false;
		selectable.GetComponent<Rigidbody>().isKinematic = false;

		sm.Deselect(selectable);
		sm.selectableItems.Remove(selectable);

		Invoke("DieSound", Random.Range(0f, 0.25f));
	}

	private void DieSound()
	{
		audioPlayer.Play("UnitDie");
	}
}
