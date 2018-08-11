using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public GameObject cubePrefab;
	public Vector2 mapSize;

	public string containerName = "Generated Map";

	[Range(0, 1)]
	public float marginPercentage;

	private float cubeSize;

	void Start()
	{
		GenerateMap();
	}

	public void GenerateMap()
	{
		if (GameObject.Find(containerName))
		{
			DestroyImmediate(GameObject.Find(containerName));
		}

		GameObject mapContainer = new GameObject(containerName);

		DetermineCubeSize();

		Vector2 relativeMapSize = new Vector2(mapSize.x * cubeSize, mapSize.y * cubeSize);

		for (float x = 0f; x < relativeMapSize.x; x += cubeSize)
		{
			for (float y = 0f; y < relativeMapSize.y; y += cubeSize)
			{
				Vector3 cubePosition = new Vector3(-relativeMapSize.x / 2 + cubeSize / 2 + x, 0, -relativeMapSize.y / 2 + cubeSize / 2 + y);
				GameObject newCube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
				newCube.transform.localScale = Vector3.one * cubeSize * (1 - marginPercentage);
				newCube.transform.parent = mapContainer.transform;
			}
		}
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
