using UnityEngine;

public class CameraController : MonoBehaviour
{
	[System.Serializable]
	public struct Range
	{
		public float min;
		public float max;
	}

	public Range xRange;
	public Range yRange;
	public Range zRange;

	public float panMargin = 10f;
	public float panSpeed = 100f;

	public float scrollSpeed = 50f;
	[Range(0, 1)]
	public float smoothSpeed;

	void Update()
	{
		Vector3 position = this.transform.position;

		if (Input.GetButton("Move Up") || Input.mousePosition.y >= Screen.height - panMargin)
		{
			position.z += panSpeed * Time.deltaTime;
		}
		if (Input.GetButton("Move Down") || Input.mousePosition.y <= panMargin)
		{
			position.z -= panSpeed * Time.deltaTime;
		}
		if (Input.GetButton("Move Left") || Input.mousePosition.x <= panMargin)
		{
			position.x -= panSpeed * Time.deltaTime;
		}
		if (Input.GetButton("Move Right") || Input.mousePosition.x >= Screen.width - panMargin)
		{
			position.x += panSpeed * Time.deltaTime;
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		position.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

		position.x = Mathf.Clamp(position.x, xRange.min, xRange.max);
		position.y = Mathf.Clamp(position.y, yRange.min, yRange.max);
		position.z = Mathf.Clamp(position.z, zRange.min, zRange.max);

		this.transform.position = Vector3.Lerp(this.transform.position, position, smoothSpeed);
	}
}
