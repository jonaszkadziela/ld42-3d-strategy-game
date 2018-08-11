using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		MapGenerator mg = (MapGenerator)target;

		mg.GenerateMap();
	}
}
