using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
	public GameObject cubePrefab;
	public Vector2 mapSize;
	public NavMeshSurface navMesh;

	public GameplayManager gameplayManager;

	public string containerName = "Generated Map";

	[Range(0, 1)]
	public float marginPercentage;

	[HideInInspector]
	public float cubeSize;

	void Start()
	{
		GenerateMap();
		UpdateNavMesh();
	}

	public void GenerateMap()
	{
		if (GameObject.Find(containerName))
		{
			DestroyImmediate(GameObject.Find(containerName));
			gameplayManager.cubesAvailable.Clear();
		}

		GameObject mapContainer = new GameObject(containerName);

		DetermineCubeSize();

		Vector2 relativeMapSize = new Vector2(mapSize.x * cubeSize, mapSize.y * cubeSize);

		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				Vector3 cubePosition = new Vector3(-relativeMapSize.x / 2 + cubeSize / 2 + x * cubeSize, -cubeSize / 2, -relativeMapSize.y / 2 + cubeSize / 2 + y * cubeSize);
				GameObject newCube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
				newCube.transform.localScale = Vector3.one * cubeSize * (1 - marginPercentage);
				newCube.transform.parent = mapContainer.transform;
				newCube.name = "Cube (" + x + ", " + y + ")";
				gameplayManager.cubesAvailable.Add(newCube);
			}
		}
	}

	public void UpdateNavMesh()
	{
		navMesh.BuildNavMesh();
	}

	private void DetermineCubeSize()
	{
		Vector3 cubeScale = cubePrefab.transform.localScale;
		cubeSize = cubeScale.x;
		Vector3 expectedCubeScale = Vector3.one * cubeSize;

		if (cubeScale != expectedCubeScale)
		{
			Debug.Log("Cube prefab is not actually a cube. Changing it to 1x1x1 cube.");
			cubeSize = 1f;
		}
	}
}
