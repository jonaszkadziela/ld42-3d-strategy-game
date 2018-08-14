using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	public GameObject unitPrefab;
	public GameObject unitParticle;

	public float increaseDifficultyLerpSpeed;

	public float increaseDifficultyDelay;
	private float increaseDifficultyDelayLeft;

	public Range unitSpawnCDRange;
	private float unitSpawnCD;
	private float unitSpawnCDLeft;

	public Range cubeDestroyCDRange;
	private float cubeDestroyCD;
	private float cubeDestroyCDLeft;

	public Range explosionPercentageRange;
	private float explosionPercentage;
	public Range cubeSelfDestructDelayRange;
	private float cubeSelfDestructDelay;
	public Range cubeExplosionDelayRange;
	private float cubeExplosionDelay;

	public List<GameObject> cubesAvailable;
	public List<GameObject> cubesToDestroy;

	private MapGenerator mg;

	void Awake()
	{
		mg = GameManager.Instance.GetComponent<MapGenerator>();
		cubesAvailable = new List<GameObject>();
		cubesToDestroy = new List<GameObject>();

		InitializeVariables();
	}

	void Update()
	{
		if (GameManager.GameOver)
		{
			return;
		}
		if (cubesAvailable.Count <= 0 || ScoreManager.TimeLeft <= 0f)
		{
			GameManager.Instance.ToggleGameOver();
			return;
		}
		if (unitSpawnCDLeft <= 0f)
		{
			Vector3 randomPosition = GetRandomPosition();

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
			GameObject cubeToDestroy = GetRandomCube();

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
		if (increaseDifficultyDelayLeft <= 0f)
		{
			RecalculateVariables(increaseDifficultyLerpSpeed);
			increaseDifficultyDelayLeft = increaseDifficultyDelay;
		}

		unitSpawnCDLeft -= Time.deltaTime;
		cubeDestroyCDLeft -= Time.deltaTime;
		increaseDifficultyDelayLeft -= Time.deltaTime;
		ScoreManager.TimeLeft -= Time.deltaTime;
	}

	public void DestroyCube(GameObject cube)
	{
		cubesToDestroy.Remove(cube);
		mg.UpdateNavMesh();
	}

	private Vector3 GetRandomPosition()
	{
		Vector3 randomPosition = Vector3.zero;
		GameObject randomCube = GetRandomCube();

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

	private GameObject GetRandomCube()
	{
		if (cubesAvailable.Count > 0)
		{
			int randomCubeIndex = Random.Range(0, cubesAvailable.Count);
			return cubesAvailable[randomCubeIndex];
		}
		return null;
	}

	private void InitializeVariables()
	{
		increaseDifficultyDelayLeft = increaseDifficultyDelay;

		unitSpawnCD = unitSpawnCDRange.max;
		cubeDestroyCD = cubeDestroyCDRange.max;
		explosionPercentage = explosionPercentageRange.min;
		cubeSelfDestructDelay = cubeSelfDestructDelayRange.max;
		cubeExplosionDelay = cubeExplosionDelayRange.max;

		unitSpawnCDLeft = unitSpawnCD;
		cubeDestroyCDLeft = cubeDestroyCD;
	}

	private void RecalculateVariables(float lerpSpeed)
	{
		unitSpawnCD = Mathf.Lerp(unitSpawnCD, unitSpawnCDRange.min, lerpSpeed);
		cubeDestroyCD = Mathf.Lerp(cubeDestroyCD, cubeDestroyCDRange.min, lerpSpeed);
		explosionPercentage = Mathf.Lerp(explosionPercentage, explosionPercentageRange.max, lerpSpeed);
		cubeSelfDestructDelay = Mathf.Lerp(cubeSelfDestructDelay, cubeSelfDestructDelayRange.min, lerpSpeed);
		cubeExplosionDelay = Mathf.Lerp(cubeExplosionDelay, cubeExplosionDelayRange.min, lerpSpeed);
	}
}
