using UnityEngine;
using System.Collections;

public class Octopus : MonoBehaviour
{
	public enum Actions
	{
		IDLE,
		SHIFT_LEFT,
		SHIFT_RIGHT,
		END
	}

	public Actions state;
	public float thinkTime, speed, turnDelay, laneDistance;

	Rigidbody rb;
	Vector3 left, right, target;
	int currentLane;
	float thinkTimer;
	bool moving;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		state = Actions.IDLE;
		left = -transform.right;
		right = transform.right;
	}

	void Update()
	{
		// Debug
		Debug.DrawLine(target, target - transform.up);

		switch (state)
		{
			case Actions.IDLE:
				thinkTimer += Time.deltaTime;

				if (thinkTimer >= thinkTime)
				{
					state = (Actions)Random.Range(0, (int)Actions.END);
					thinkTimer = 0;
				}
				break;
			case Actions.SHIFT_LEFT:
				MoveTo(left);
				break;
			case Actions.SHIFT_RIGHT:
				MoveTo(right);
				break;
			default:
				state = Actions.IDLE;
				break;
		}
	}

	void MoveTo(Vector3 direction)
	{
		if (!moving)
		{
			target = transform.position + direction * laneDistance;

			// Check if solid ground
			if (Physics.Raycast(target, -transform.up, 1))
			{
				moving = true;
			}
			else
			{
				state = Actions.IDLE;
			}
		}
		else
		{
			// Set facing
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime / turnDelay);

			// Movement
			Vector2 move = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
			move.y = transform.position.y; // Retain Y position
			transform.position = move;

			if (transform.position == target)
			{
				state = Actions.IDLE;
				moving = false;
			}
		}
	}
}