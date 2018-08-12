using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	public GameObject unitPrefab;
	public GameObject unitParticle;

	public float unitSpawnDelay = 2f;
	private float unitSpawnDelayLeft = 0f;

	public List<GameObject> cubesList;

	void Awake()
	{
		cubesList = new List<GameObject>();
	}

	void Update()
	{
		if (unitSpawnDelayLeft <= 0f)
		{
			GameObject newUnit = Instantiate(unitPrefab, getRandomCubePosition(), Quaternion.identity);
			Instantiate(unitParticle, newUnit.transform.position, Quaternion.identity);
			unitSpawnDelayLeft = unitSpawnDelay;
		}

		unitSpawnDelayLeft -= Time.deltaTime;
	}

	private Vector3 getRandomCubePosition()
	{
		Vector3 cubePosition = Vector3.zero;
		GameObject randomCube;

		int randomCubeIndex = Random.Range(0, cubesList.Count);

		randomCube = cubesList[randomCubeIndex];

		if (randomCube != null)
		{
			cubePosition = randomCube.transform.position;
		}

		return cubePosition;
	}
}
