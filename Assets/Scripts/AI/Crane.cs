﻿using UnityEngine;
using System.Collections;

public class Crane : MonoBehaviour
{
	public enum Actions
	{
		//IDLE,
		//FORWARD,
		LEFT,
		RIGHT,
		END
	}

	public Actions state;
	public float /*thinkTime, speed,*/ turnSpeed;// turnDelay;

	Vector3 forward, left, right, target;
	float thinkTimer;

	void Start()
	{
		state = (Actions)Random.Range(0, (int)Actions.END);
		forward = transform.forward;
		left = -transform.right;
		right = transform.right;

		switch (state)
		{
			//case Actions.FORWARD:
			//	SetRotation(forward);
			//	break;
			case Actions.LEFT:
				SetRotation(left);
				break;
			case Actions.RIGHT:
				SetRotation(right);
				break;
			//default:
			//	state = Actions.IDLE;
			//	break;
		}
	}

	void Update()
	{
		switch (state)
		{
			//case Actions.IDLE:
			//	thinkTimer += Time.deltaTime;

			//	if (thinkTimer >= thinkTime)
			//	{
			//		state = (Actions)Random.Range(0, (int)Actions.END);
			//		thinkTimer = 0;
			//	}
			//	break;
			//case Actions.FORWARD:
			//	TurnTo(forward);
			//	break;
			case Actions.LEFT:
				Turn(turnSpeed);
				break;
			case Actions.RIGHT:
				Turn(-turnSpeed);
				break;
			//default:
			//	state = Actions.IDLE;
			//	break;
		}
	}

	void Turn(float speed)
	{
		// Set facing
		//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime / turnDelay);

		//if (transform.rotation == Quaternion.LookRotation(direction))
		//{
		//state = Actions.IDLE;
		//}
		transform.Rotate(0, speed * Time.deltaTime, 0);
	}

	void SetRotation(Vector3 direction)
	{
		transform.rotation = Quaternion.LookRotation(direction);
		//state = Actions.IDLE;
	}
}
