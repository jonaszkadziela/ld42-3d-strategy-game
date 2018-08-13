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
	public float lerpSpeed;

	public static Vector3 InitialPosition;

	private Vector3 moveToTargetPosition;
	private float moveToTargetLerpSpeed;
	private bool moveToTarget = false;

	void Start()
	{
		InitialPosition = transform.position;
	}

	void Update()
	{
		if (GameManager.GameOver && !moveToTarget)
		{
			return;
		}

		Vector3 position = transform.position;
		float speed = lerpSpeed;

		if (!moveToTarget)
		{
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
		}
		else
		{
			position = moveToTargetPosition;
			speed = moveToTargetLerpSpeed;
		}

		transform.position = Vector3.Lerp(transform.position, position, speed);

		if (moveToTarget && transform.position == position)
		{
			moveToTarget = false;
		}
	}

	public void MoveCameraToTarget(Vector3 position, float lerpSpeed)
	{
		moveToTarget = true;
		moveToTargetPosition = position;
		moveToTargetLerpSpeed = lerpSpeed;
	}
}
