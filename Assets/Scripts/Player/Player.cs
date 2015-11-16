using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public enum PlayerMode
	{
		RUNNING,
		TURNING
	}

	public PlayerMode playerMode;
	public int drunkenness;
	public int rumStrength;
	public int waterStrength;
	public float minRunSpeed, maxRunSpeed;
	public float minJumpHeight, maxJumpHeight;
	public float minLaneDelay, maxLaneDelay;
	public float laneDistance;
	public int currentLane;
	public bool isTurning;
	public LevelGen lg;

	private SceneManager_Andrew sceneManager = null;
	private Vector3 startingPosition = Vector3.zero;
	private Quaternion startingRotation = Quaternion.identity;
	private Rigidbody rb;
	private bool dead = false;
	// Controls
	private bool actionLeft, actionRight, actionJump;
	// Lane Switching
	private float laneVelocity;
	private float lanePosition;
	private int previousLane;
	private bool jumping;
	// Corner Turning
	private Vector3 cornerStart;
    private Vector3 cornerPoint;
	private Vector3 cornerEnd;
	private float turnTimer;
	private float turnDegree;
	private float turnAngle;
	// Touch
	public float swipeDistance = 5, swipeTime = 0.75f;
	private bool couldBeSwipe;
	private Vector2 swipeStartPos;
	private float swipeStartTime;

	void Start()
	{
		sceneManager = SceneManager_Andrew.instance;
		startingRotation = transform.rotation;
		startingPosition = transform.position;
		rb = GetComponent<Rigidbody>();
		lg = GameObject.FindGameObjectWithTag ("LevelGen").GetComponent<LevelGen>();
		ResetCharacter();
	}

	void Update()
	{
		if (!dead)
		{
			Controls();
			Action();
			Limits();
		}

		// Debug
		if (cornerPoint != Vector3.zero && cornerEnd != Vector3.zero)
		{
			Debug.DrawLine(cornerStart, cornerPoint, Color.red);
			Debug.DrawLine(cornerPoint, cornerEnd, Color.red);
		}
	}

	void FixedUpdate()
	{
		if (!dead)
		{
			Movement();
		}
	}

	void Controls()
	{
		// PC Controls
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			actionLeft = true;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			actionRight = true;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow) && !jumping)
		{
			actionJump = true;
		}

		// Touch Controls
		CheckHorizontalSwipes();
	}

	void CheckHorizontalSwipes()
	{
		foreach (Touch touch in Input.touches)
		{ // For every touch in the Input.touches - array...

			switch (touch.phase)
			{
				case TouchPhase.Began: // The finger first touched the screen --> It could be(come) a swipe
					couldBeSwipe = true;

					swipeStartPos = touch.position;  // Position where the touch started
					swipeStartTime = Time.time; // The time it started
					break;

				case TouchPhase.Stationary: // Is the touch stationary? --> No swipe then!
					couldBeSwipe = false;
					break;
			}

			float time = Time.time - swipeStartTime; // Time the touch stayed at the screen till now.
			float distance = Mathf.Abs(touch.position.x - swipeStartPos.x); //Swipe distance

			if (couldBeSwipe && time < swipeTime && distance > swipeDistance)
			{
				// It's a swiiiiiiiiiiiipe!
				couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.

				if (Mathf.Sign(touch.position.x - swipeStartPos.x) == 1f) //Swipe-direction, either 1 or -1.
				{
					//Right-swipe
					actionRight = true;
				}
				else
				{
					// Left-swipe
					actionLeft = true;
				}
			}
		}
	}

	void Action()
	{
		switch (playerMode)
		{
			case PlayerMode.RUNNING:

				if (actionLeft)
				{
					previousLane = currentLane;
					currentLane--;
				}

				if (actionRight)
				{
					previousLane = currentLane;
					currentLane++;
				}

				if (actionJump)
				{
					jumping = true;
				}
				break;

			case PlayerMode.TURNING:

				if (isTurning)
				{
					if (cornerStart == Vector3.zero)
					{
						cornerStart = transform.position;// cornerPoint + -transform.forward * Vector3.Distance(transform.position, cornerPoint);
                    }

					turnTimer += Time.deltaTime;

					// Position
					Vector3 point1 = Vector3.Lerp(cornerStart, cornerPoint, turnTimer);
					Vector3 point2 = Vector3.Lerp(cornerPoint, cornerEnd, turnTimer);

					transform.position = Vector3.Lerp(point1, point2, turnTimer);

					// Rotation
					//turnAngle = Mathf.LerpAngle(transform.rotation.y, turnDegree, turnTimer);
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, turnDegree, 0), turnTimer);

					// Stop Turning
					if (turnTimer >= 1)
					{
						currentLane = 0;
						laneVelocity = 0;
						lanePosition = 0;

						isTurning = false;
					}
				}

				if (actionLeft && !isTurning)
				{
					cornerEnd = cornerPoint + -transform.right * 10;
					turnDegree -= 90.0f;

					isTurning = true;
                }

				if (actionRight && !isTurning)
				{
					cornerEnd = cornerPoint + transform.right * 10;
					turnDegree += 90.0f;

					isTurning = true;
				}
				break;
		}

		actionLeft = false;
		actionRight = false;
		actionJump = false;
	}

	void Limits()
	{
		if (currentLane > 2)
		{
			currentLane = 2;
		}

		if (currentLane < -2)
		{
			currentLane = -2;
		}

		if (drunkenness > 100)
		{
			drunkenness = 100;
		}

		if (drunkenness < 0)
		{
			drunkenness = 0;
		}

		if (playerMode == PlayerMode.TURNING)
		{
			rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
		else
		{
			rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
		}

		if (cornerPoint != Vector3.zero)
		{
			cornerPoint.y = transform.position.y;
		}

		if (turnDegree >= 360)
		{
			turnDegree = 0;
		}

		if (turnDegree <= -360)
		{
			turnDegree = 0;
		}
	}

	void Movement()
	{
		Vector3 pos = transform.position;

		// Lane Hopping
		float laneDelay = Mathf.Lerp(minLaneDelay, maxLaneDelay, drunkenness / 100.0f);

		lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

		pos += transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

		// Running
		float runSpeed = Mathf.Lerp(minRunSpeed, maxRunSpeed, drunkenness / 100.0f);

		pos += (transform.forward * runSpeed) * Time.deltaTime;

		if (sceneManager != null)
		{
			sceneManager.m_Distance += runSpeed * Time.deltaTime;
		}

		// Jumping
		float jumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, drunkenness / 100.0f);

		if (jumping)
		{
			pos += (transform.up * jumpHeight) * Time.deltaTime;
		}

		transform.position = pos;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (sceneManager != null && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
		{
			sceneManager.Die();
			ResetCharacter();
			if (sceneManager.m_Lives <= 0)
			{
				dead = true;
			}
		}

		//else
		//{
		//	// Detect direction of collision
		//	Vector3 hit = a_Collision.contacts[0].normal;
		//	Debug.Log(hit);
		//	float angle = Vector3.Angle(hit, Vector3.up);

		//	if (Mathf.Approximately(angle, 0))
		//	{
		//		//Down
		//		Debug.Log("Down");
		//	}
		//	if (Mathf.Approximately(angle, 180))
		//	{
		//		//Up
		//		Debug.Log("Up");
		//	}
		//	if (Mathf.Approximately(angle, 90)) // Sides
		//	{
		//		Vector3 cross = Vector3.Cross(Vector3.forward, hit);
		//		if (cross.y == 1)
		//		{
		//			// Left side
		//			Debug.Log("Left");

		//			// Stop from running into wall
		//			currentLane = previousLane;
		//		}
		//		else if (cross.y == -1)
		//		{
		//			// Right side
		//			Debug.Log("Right");

		//			// Stop from running into wall
		//			currentLane = previousLane;
		//		}
		//	}
		//}

		jumping = false;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("CornerTrigger"))
		{
			//currentLane = 0;
			jumping = false;

			cornerPoint = collider.gameObject.transform.position;

			playerMode = PlayerMode.TURNING;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("CornerTrigger"))
		{
			cornerStart = Vector3.zero;
			cornerPoint = Vector3.zero;
			cornerEnd = Vector3.zero;
			turnTimer = 0;

			playerMode = PlayerMode.RUNNING;

			isTurning = false;
		}
	}

	public void ResetCharacter()
	{
		rb.velocity = Vector3.zero;
		rb.transform.position = startingPosition;
		rb.rotation = startingRotation;

		transform.position = startingPosition;
		transform.rotation = startingRotation;

		currentLane = 0;
		laneVelocity = 0;
		lanePosition = 0;

		cornerStart = Vector3.zero;
		cornerPoint = Vector3.zero;
		cornerEnd = Vector3.zero;
		turnTimer = 0;
		turnAngle = 0;
		turnDegree = 0;

	drunkenness = 0;

		lg.RebuildMap ();
	}

	public void GetDrunk()
	{
		drunkenness += rumStrength;
	}

	public void SoberUp()
	{
		drunkenness -= waterStrength;
	}
}