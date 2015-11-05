using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public int drunkenness;
	public int rumStrength;
	public int waterStrength;
	public float minRunSpeed, maxRunSpeed;
	public float minJumpHeight, maxJumpHeight;
	public float minLaneDelay, maxLaneDelay;
	public float laneDistance;
	public int currentLane;

	private SceneManager_Andrew _mySceneManager = null;
    private Transform _PlayerTransform = null;
    private Vector3 _StartingPosition = Vector3.zero;
	private Quaternion _StartingRotation = Quaternion.identity;
	private Rigidbody rb;
	private float laneVelocity;
	private float lanePosition;
	private bool jumping;
    private bool dead = false;

	// Touch
	public float minSwipeDist, maxSwipeTime;

	private bool couldBeSwipe;
	private Vector2 swipeStartPos;
	private float swipeStartTime;

	void Start()
	{
		_mySceneManager = SceneManager_Andrew.instance;
        _PlayerTransform = transform;
        _StartingPosition = transform.position;
		rb = GetComponent<Rigidbody>();

		ResetCharacter();
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

		if (Input.GetKeyDown(KeyCode.UpArrow) && !jumping)
		{
			jumping = true;
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

			float swipeTime = Time.time - swipeStartTime; // Time the touch stayed at the screen till now.
			float swipeDist = Mathf.Abs(touch.position.x - swipeStartPos.x); //Swipe distance

			if (couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist)
			{
				// It's a swiiiiiiiiiiiipe!
				couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.

				if (Mathf.Sign(touch.position.x - swipeStartPos.x) == 1f) //Swipe-direction, either 1 or -1.
				{
					//Right-swipe
					currentLane++;
				}
				else
				{
					// Left-swipe
					currentLane--;
				}
			}
		}
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
	}

	void Movement()
	{
        if (dead) return;
		Vector3 pos = transform.position;

		// Lane Hopping
		float laneDelay = Mathf.Lerp(minLaneDelay, maxLaneDelay, drunkenness / 100);

		lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

		pos += transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

		// Running
		float runSpeed = Mathf.Lerp(minRunSpeed, maxRunSpeed, drunkenness / 100);

		pos += (transform.forward * runSpeed) * Time.deltaTime;

        if (_mySceneManager != null)
        {
            _mySceneManager.m_Distance += runSpeed * Time.deltaTime;
        }

		// Jumping
		if (jumping)
		{
			pos += (transform.up * minJumpHeight) * Time.deltaTime;
		}

		transform.position = pos;
	}

    void OnCollisionEnter(Collision a_Collision)
    {
        if (_mySceneManager != null && a_Collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _mySceneManager.Die();
            ResetCharacter();
            if (_mySceneManager.m_Lives <= 0)
            {
                dead = true;
            }
        }

		jumping = false;
    }

    public void ResetCharacter()
    {
		rb.velocity = Vector3.zero;
		rb.transform.position = _StartingPosition;
		rb.rotation = _StartingRotation;

        _PlayerTransform.position = _StartingPosition;
		transform.rotation = _StartingRotation;

		currentLane = 0;
		laneVelocity = 0;
		lanePosition = 0;

		drunkenness = 0;
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