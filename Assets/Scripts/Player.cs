using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float runSpeed;
	public float laneDelay;
	public float laneDistance;
	public int currentLane;

	private Rigidbody rb;
	private Vector3 velocity = Vector3.zero;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
    }
	
	void Update()
	{
		Controls();

		Limits();
	}

	void FixedUpdate()
	{
		Movement();
	}

	void Controls()
	{
		// PC Controls
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			currentLane--;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			currentLane++;
		}
	}

	void Limits()
	{
		if (currentLane > 1)
		{
			currentLane = 1;
		}

		if (currentLane < -1)
		{
			currentLane = -1;
		}
	}

	void Movement()
	{
		Vector3 pos = transform.position;

		// Lane Hopping
		float targetX = currentLane * laneDistance;

		pos.x = Mathf.SmoothDamp(transform.position.x, targetX, ref velocity.x, laneDelay);

		// Running
		pos += (transform.forward * runSpeed) * Time.deltaTime;

		transform.position = pos;
    }
}