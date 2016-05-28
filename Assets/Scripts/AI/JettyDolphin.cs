using UnityEngine;
using System.Collections;

public class JettyDolphin : MonoBehaviour
{
	public float leapSpeed;
	public bool leaping;

	Quaternion startRot;

	void Start()
	{
		startRot = transform.localRotation;

		Reset();
	}

	void Update()
	{
		if (leaping)
		{
			transform.Rotate(0, 0, leapSpeed * Time.deltaTime);

			if (transform.eulerAngles.z >= 270.0f)
			{
				Reset();
			}
		}
	}

	void Reset()
	{
		transform.localRotation = startRot;
		//leaping = false;

		// Randomly flip
		if (Random.Range(0, 2) == 1)
		{
			Flip();
		}
	}

	void Flip()
	{
		transform.Rotate(0, 180.0f, 0);
	}
}
