using UnityEngine;

public class ParticleCleaner : MonoBehaviour
{
	private ParticleSystem particle;

	void Start()
	{
		particle = GetComponent<ParticleSystem>();
	}

	void Update()
	{
		if (particle.isStopped)
		{
			Destroy(gameObject);
		}
	}
}
