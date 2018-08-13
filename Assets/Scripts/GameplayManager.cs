using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	public GameObject unitPrefab;
	public GameObject unitParticle;

	public float unitSpawnCD = 2f;
	private float unitSpawnCDLeft;

	public float cubeDestroyCD = 2f;
	private float cubeDestroyCDLeft;

	[Range(0, 1)]
	public float explosionPercentage = 0.25f;
	public float cubeSelfDestructDelay = 1f;
	public float cubeExplosionDelay = 2f;

	public List<GameObject> cubesAvailable;
	public List<GameObject> cubesToDestroy;

	private MapGenerator mg;

	void Awake()
	{
		mg = GameManager.Instance.GetComponent<MapGenerator>();
		cubesAvailable = new List<GameObject>();
		cubesToDestroy = new List<GameObject>();
		unitSpawnCDLeft = unitSpawnCD;
		cubeDestroyCDLeft = cubeDestroyCD;
	}

	void Update()
	{
		if (GameManager.Instance.gameOver)
		{
			return;
		}
		if (cubesAvailable.Count <= 0 || ScoreManager.TimeLeft <= 0f)
		{
			GameManager.Instance.GameOver();
			return;
		}
		if (unitSpawnCDLeft <= 0f)
		{
			Vector3 randomPosition = getRandomPosition();

			if (randomPosition == Vector3.zero)
			{
				return;
			}

			GameObject newUnit = Instantiate(unitPrefab, randomPosition, Quaternion.identity);
			Instantiate(unitParticle, newUnit.transform.position, Quaternion.identity);

			ScoreManager.ChangeCurrentUnits(1);

			unitSpawnCDLeft = unitSpawnCD;
		}
		if (cubeDestroyCDLeft <= 0f)
		{
			GameObject cubeToDestroy = getRandomCube();

			if (cubeToDestroy == null)
			{
				return;
			}

			Cube.DestroyTypes destroyType = Cube.DestroyTypes.SelfDestruct;
			float destroyDelay = cubeSelfDestructDelay;

			if (explosionPercentage >= Random.Range(0f, 1f))
			{
				destroyType = Cube.DestroyTypes.Explode;
				destroyDelay = cubeExplosionDelay;
			}

			cubeToDestroy.GetComponent<Cube>().InvokeDestroy(destroyDelay, destroyType);
			cubesAvailable.Remove(cubeToDestroy);
			cubesToDestroy.Add(cubeToDestroy);

			cubeDestroyCDLeft = cubeDestroyCD;
		}

		unitSpawnCDLeft -= Time.deltaTime;
		cubeDestroyCDLeft -= Time.deltaTime;
		ScoreManager.TimeLeft -= Time.deltaTime;
	}

	public void DestroyCube(GameObject cube)
	{
		cubesToDestroy.Remove(cube);
		mg.UpdateNavMesh();
	}

	private Vector3 getRandomPosition()
	{
		Vector3 randomPosition = Vector3.zero;
		GameObject randomCube = getRandomCube();

		if (randomCube != null)
		{
			float offset = mg.cubeSize / 2;

			randomPosition = randomCube.transform.position;
			randomPosition.x += Random.Range(-offset, offset);
			randomPosition.y += offset;
			randomPosition.z += Random.Range(-offset, offset);
		}

		return randomPosition;
	}

	private GameObject getRandomCube()
	{
		if (cubesAvailable.Count > 0)
		{
			int randomCubeIndex = Random.Range(0, cubesAvailable.Count);
			return cubesAvailable[randomCubeIndex];
		}
		return null;
	}
}
