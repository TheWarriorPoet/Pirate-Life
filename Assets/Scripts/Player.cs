using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float laneSpeed;
	public float laneDistance;
	public int currentLane;

	private Vector3 startPos;
	private Vector3 velocity;

	void Start()
	{
		startPos = transform.position;
		velocity = Vector3.zero;
    }
	
	void Update()
	{
		Controls();

		Limits();
	}

	void LateUpdate()
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
		Vector3 targetPosition = startPos + new Vector3(currentLane * laneDistance, 0, 0);

		Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, laneSpeed);

		transform.position = newPosition;
	}
}