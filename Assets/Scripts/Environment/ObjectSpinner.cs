using UnityEngine;
using System.Collections;

public class ObjectSpinner : MonoBehaviour
{
	public float spinSpeed;
	public bool randomStartRotation;

	void Start()
	{
		if (randomStartRotation)
		{
			transform.Rotate(transform.up, Random.Range(0.0f, 360.0f));
		}
	}

	void Update()
	{
		transform.Rotate(transform.up, spinSpeed * Time.deltaTime);
	}
}
