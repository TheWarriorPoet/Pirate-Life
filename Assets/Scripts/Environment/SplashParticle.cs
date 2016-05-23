using UnityEngine;
using System.Collections;

public class SplashParticle : MonoBehaviour
{
	ParticleSystem ps;
	public Vector3 rotationOffset;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		transform.rotation = Quaternion.Euler(rotationOffset);
	}

	void Update()
	{
		if (ps.isPlaying)
		{
			ps.startRotation = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
			ps.startSize = Random.Range(0.25f, 1.0f);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}