using UnityEngine;
using UnityEngine.AI;

public class ShowPath : MonoBehaviour
{
	public NavMeshAgent agent;
	public LineRenderer line;
	public GameObject targetIndicator;

	public Color lineColor;
	public float lineWidth = 0.05f;

	void Start()
	{
		line.startWidth = lineWidth;
		line.endWidth = lineWidth;
		line.startColor = lineColor;
		line.endColor = lineColor;
	}

	void Update()
	{
		NavMeshPath path = agent.path;

		if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
		{
			if (!targetIndicator.activeInHierarchy)
			{
				targetIndicator.SetActive(true);
			}

			targetIndicator.transform.position = agent.pathEndPosition;

			line.positionCount = path.corners.Length;

			for (int i = 0; i < line.positionCount; i++)
			{
				line.SetPosition(i, path.corners[i]);
			}
		}
		else
		{
			line.positionCount = 0;
			targetIndicator.SetActive(false);
		}
	}
}
