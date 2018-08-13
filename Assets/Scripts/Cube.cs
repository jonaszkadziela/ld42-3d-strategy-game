using UnityEngine;
using UnityEngine.AI;

public class Cube : MonoBehaviour
{
	public enum DestroyTypes
	{
		SelfDestruct,
		Explode,
	};

	public LayerMask unitMask;
	public LayerMask cubeMask;
	public GameObject cubeExplosionParticle;
	public Color selfDestructColor;
	public Color explodeColor;
	public float blinkSpeed = 4f;
	public float cubeExplosionRadius = 2f;

	private Rigidbody rb;
	private MeshRenderer mr;
	private GameplayManager gm;
	private Color initialColor;
	private Color targetColor;
	private bool toDestroy = false;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		mr = GetComponent<MeshRenderer>();
		gm = GameManager.Instance.GetComponent<GameplayManager>();
		initialColor = mr.material.color;
	}

	void Update()
	{
		if (toDestroy)
		{
			mr.material.color = Color.Lerp(initialColor, targetColor, Mathf.PingPong(Time.time * blinkSpeed, 0.75f));
		}
		else
		{
			mr.material.color = initialColor;
		}
	}

	public void InvokeDestroy(float delay, DestroyTypes type = DestroyTypes.SelfDestruct)
	{
		toDestroy = true;
		gm.cubesAvailable.Remove(gameObject);
		gm.cubesToDestroy.Add(gameObject);
		switch (type)
		{
			case DestroyTypes.SelfDestruct:
				targetColor = selfDestructColor;
				Invoke("SelfDestruct", delay);
			break;
			case DestroyTypes.Explode:
				targetColor = explodeColor;
				Invoke("Explode", delay);
			break;
		}
	}

	public void CancelDestroy()
	{
		toDestroy = false;
		gm.cubesAvailable.Add(gameObject);
		gm.cubesToDestroy.Remove(gameObject);
		CancelInvoke("SelfDestruct");
		CancelInvoke("Explode");
	}

	private void SelfDestruct()
	{
		if (!toDestroy)
		{
			return;
		}

		Vector3 overlapBoxScale = transform.localScale / 2;
		overlapBoxScale.y *= 2;

		Collider[] units = Physics.OverlapBox(transform.position, overlapBoxScale, Quaternion.identity, unitMask);

		foreach (Collider unit in units)
		{
			unit.transform.parent.GetComponent<Unit>().enabled = false;
		}

		ScoreManager.ChangeCurrentUnits(-units.Length);

		rb.isKinematic = false;
		gameObject.layer = 0;
		toDestroy = false;

		gm.DestroyCube(gameObject);
	}

	private void Explode()
	{
		if (!toDestroy)
		{
			return;
		}

		Instantiate(cubeExplosionParticle, transform.position, Quaternion.identity);

		Collider[] cubes = Physics.OverlapSphere(transform.position, cubeExplosionRadius, cubeMask);

		foreach (Collider cube in cubes)
		{
			cube.gameObject.GetComponent<Cube>().InvokeDestroy(0);
		}
	}
}
