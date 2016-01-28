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
    public float drunkDelay;
    public int rumStrength;
    public int waterStrength;
    public float minRunSpeed, maxRunSpeed;
    public float minJumpHeight, maxJumpHeight;
    public float minLaneDelay, maxLaneDelay;
    public float laneDistance;
    public int currentLane;
	public bool jumping, falling;
	public bool isTurning;
    public LevelGen lg;

	private Camera mainCamera;
	private Animator anim;
	private PirateCharacterAnimator ragdoll;
	private SceneManager_Andrew sceneManager = null;
    private Vector3 startingPosition = Vector3.zero;
    private Quaternion startingRotation = Quaternion.identity;
    private Rigidbody rb;
    private bool dead = false, ragdolled = false;
    private AudioClip jumpSound, deckSound, landSound, splashSound, smackSound;
    // Controls
    private bool actionLeft, actionRight, actionJump;
    // Drunkenness
    private float prevDrunkenness;
    private float newDrunkenness;
    private float drunkTimer;
    // Lane Switching
    private float laneVelocity;
    private float lanePosition;
    private int previousLane;
    // Corner Turning
    private Vector3 cornerStart;
    private Vector3 cornerPoint;
    private Vector3 cornerEnd;
    private float turnTimer;
    private float turnDegree;
    // Touch
    public float swipeDistance = 5, swipeTime = 0.75f;
    private bool couldBeSwipe;
    private Vector2 swipeStartPos;
    private float swipeStartTime;

    void Start()
    {
		anim = GetComponentInChildren<Animator>();
		mainCamera = GetComponentInChildren<Camera>();
		ragdoll = GetComponentInChildren<PirateCharacterAnimator>();
		sceneManager = SceneManager_Andrew.instance;
        startingRotation = transform.rotation;
        startingPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        lg = GameObject.FindGameObjectWithTag("LevelGen").GetComponent<LevelGen>();

        jumpSound = (AudioClip)Resources.Load("Sounds/player_jump");
        splashSound = (AudioClip)Resources.Load("Sounds/player_splash");
		smackSound = (AudioClip)Resources.Load("Sounds/player_smack");
		deckSound = (AudioClip)Resources.Load("Sounds/deck_jump");
		landSound = (AudioClip)Resources.Load("Sounds/player_land");

		ResetCharacter();
    }

    void Update()
    {
        if (!dead && !ragdolled)
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
        if (!dead && !ragdolled)
        {
            Movement();
        }
    }

    void Controls()
    {
        // PC Controls
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            actionLeft = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            actionRight = true;
        }

        if (!falling && Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && !jumping)
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

                if (actionJump && !jumping)
                {
                    AudioSource.PlayClipAtPoint(jumpSound, transform.position);
					AudioSource.PlayClipAtPoint(deckSound, transform.position);
					anim.Play("Jumping");

					jumping = true;
                }
                break;

            case PlayerMode.TURNING:

                if (isTurning)
                {
					// Calculate curve
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
                    cornerEnd = cornerPoint + -transform.right * 4.5f;
                    turnDegree -= 90.0f;

                    isTurning = true;
                }

                if (actionRight && !isTurning)
                {
                    cornerEnd = cornerPoint + transform.right * 4.5f;
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

        if (newDrunkenness > 100)
        {
            newDrunkenness = 100;
        }

        if (newDrunkenness < 0)
        {
            newDrunkenness = 0;
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
		// Camera effects
		mainCamera.fieldOfView = 60.0f + drunkenness / 5.0f;
		mainCamera.transform.localEulerAngles = new Vector3(15, 0, laneVelocity * drunkenness / 75.0f);

		// Falling
		if (rb.velocity.y < -0.15f && !falling)
		{
			falling = true;
		}
		else
		{
			falling = false;
		}

        Vector3 pos = transform.position;

        // Drunkenness
        if (drunkenness != newDrunkenness)
        {
            drunkTimer += Time.deltaTime / drunkDelay;
            drunkenness = (int)Mathf.Lerp(prevDrunkenness, newDrunkenness, drunkTimer);
        }

        // Lane Hopping
        float laneDelay = Mathf.Lerp(minLaneDelay, maxLaneDelay, drunkenness / 100.0f);

        lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

        pos += transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

        // Leaning
        transform.Find("Pirate_Character").localEulerAngles = new Vector3(0, 0, -laneVelocity * (1.0f + drunkenness / 20.0f));

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

	void KillCharacter()
	{
		sceneManager.Die();
		ragdoll.GoToRagdoll();
		ragdolled = true;
		mainCamera.transform.SetParent(null);

		if (sceneManager.m_Lives <= 0)
		{
			dead = true;
			//Renderer[] renderers = GetComponentsInChildren<Renderer>();

			//foreach (var renderer in renderers)
			//{
			//	renderer.enabled = false;
			//}
		}
		else
		{
			Invoke("ResetCharacter", 2.0f);
		}
	}

    void OnCollisionEnter(Collision collision)
    {
        if (sceneManager != null && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && !ragdolled)
        {
			AudioSource.PlayClipAtPoint(smackSound, transform.position);
			KillCharacter();
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

		if (jumping)
		{
			if (!ragdolled)
			{
				AudioSource.PlayClipAtPoint(landSound, transform.position);
				anim.Play("Falling");
			}
			jumping = false;
		}
    }

    void OnTriggerEnter(Collider collider)
    {
		if (collider.gameObject.layer == LayerMask.NameToLayer("GameWater") && !ragdolled)
		{
			AudioSource.PlayClipAtPoint(splashSound, transform.position);
			KillCharacter();
		}

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

			// Avoid flying
			AudioSource.PlayClipAtPoint(landSound, transform.position);
			anim.Play("Falling");

			isTurning = false;
        }
    }

    public void ResetCharacter()
    {
		mainCamera.transform.SetParent(transform);

		if (ragdolled)
		{
			ragdoll.ResetRagdoll();
			ragdolled = false;
		}

		rb.velocity = Vector3.zero;
        rb.transform.position = startingPosition;
        rb.rotation = startingRotation;

        transform.position = startingPosition;
        transform.rotation = startingRotation;

		mainCamera.transform.position = transform.position + new Vector3(0, 3.75f, -4.5f);
		mainCamera.transform.rotation = Quaternion.Euler(15, 0, 0);

		currentLane = 0;
        laneVelocity = 0;
        lanePosition = 0;

        cornerStart = Vector3.zero;
        cornerPoint = Vector3.zero;
        cornerEnd = Vector3.zero;
        turnTimer = 0;
        turnDegree = 0;

        jumping = false;

        drunkenness = 0;
        drunkTimer = 0;
        prevDrunkenness = 0;
        newDrunkenness = 0;

        lg.RebuildMap();

		anim.Play("Movement");
	}

    public void GetDrunk()
    {
        prevDrunkenness = drunkenness;
        newDrunkenness += rumStrength;
        drunkTimer = 0;
    }

    public void SoberUp()
    {
        prevDrunkenness = drunkenness;
        newDrunkenness -= waterStrength;
        drunkTimer = 0;
    }
}