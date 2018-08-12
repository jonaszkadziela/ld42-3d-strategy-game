using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	public GameObject unitPrefab;
	public GameObject unitParticle;

	public float unitSpawnDelay = 2f;
	private float unitSpawnDelayLeft = 2f;

	public float cubeDestroyDelay;
	private float cubeDestroyDelayLeft;

	public List<GameObject> cubesList;

	private MapGenerator map;

	void Awake()
	{
		map = GameManager.Instance.GetComponent<MapGenerator>();
		cubesList = new List<GameObject>();
		unitSpawnDelayLeft = unitSpawnDelay;
		cubeDestroyDelayLeft = cubeDestroyDelay;
	}

	void Update()
	{
		if (GameManager.Instance.gameOver)
		{
			return;
		}
		if (cubesList.Count <= 0)
		{
			GameManager.Instance.GameOver();
			return;
		}
		if (unitSpawnDelayLeft <= 0f)
		{
			Vector3 randomPosition = getRandomCubePosition();
			randomPosition.y += map.cubeSize / 2;

			if (randomPosition == Vector3.zero)
			{
				return;
			}

			GameObject newUnit = Instantiate(unitPrefab, randomPosition, Quaternion.identity);
			Instantiate(unitParticle, newUnit.transform.position, Quaternion.identity);

			unitSpawnDelayLeft = unitSpawnDelay;
		}
		if (cubeDestroyDelayLeft <= 0f)
		{
			GameObject randomCube = getRandomCube();

			if (randomCube == null)
			{
				return;
			}

			cubesList.Remove(randomCube);
			randomCube.layer = 0;
			randomCube.GetComponent<Cube>().SelfDestruct();
			map.UpdateNavMesh();

			cubeDestroyDelayLeft = cubeDestroyDelay;
		}

		unitSpawnDelayLeft -= Time.deltaTime;
		cubeDestroyDelayLeft -= Time.deltaTime;
	}

	private Vector3 getRandomCubePosition()
	{
		Vector3 cubePosition = Vector3.zero;
		GameObject randomCube = getRandomCube();

		if (randomCube != null)
		{
			cubePosition = randomCube.transform.position;
		}

		return cubePosition;
	}

	private GameObject getRandomCube()
	{
		if (cubesList.Count > 0)
		{
			int randomCubeIndex = Random.Range(0, cubesList.Count);
			return cubesList[randomCubeIndex];
		}
		return null;
	}
}
