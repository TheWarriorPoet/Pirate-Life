using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public enum PlayerMode
    {
        RUNNING,
        TURNING
    }

	[Header("Player Settings")]
	public PlayerMode playerMode;
    public int drunkenness;
    public float drunkDelay;
    public float minRunSpeed, maxRunSpeed;
    public float minJumpHeight, maxJumpHeight;
    public float minLaneDelay, maxLaneDelay;
    public float laneDistance;
	public int currentLane;
	public bool jumping, isTurning;
	[Header("Touch Settings")]
	public float deadzone = 0.8f;
	[Header("Object Linking")]
	public LevelGen lg;

	private Camera mainCamera;
	private Animator anim;
	private PirateCharacterAnimator ragdoll;
	private SceneManager_Andrew sceneManager = null;
    private Vector3 startingPosition = Vector3.zero;
    private Quaternion startingRotation = Quaternion.identity;
    private CharacterController controller;
    private bool dead = false, ragdolled = false;
    private AudioClip jumpSound, deckSound, landSound, splashSound, smackSound;
    // Controls
    private bool actionLeft, actionRight, actionJump;
	private float jumpVelocity, jumpSpeed, runSpeed;
	private Vector3 velocity;
	List<Vector3> arc = new List<Vector3>();
	// Drunkenness
	private int prevDrunkenness;
	private int newDrunkenness;
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
	Vector2 touchDelta, touchPrevious;
    private bool swiping;
	Text debugText; // Quick and dirty debugging

	private static Player _instance = null;
    public static Player instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (Player)FindObjectOfType(typeof(Player));
            }
            return _instance;
        }
    }
    void Start()
    {
		anim = GetComponentInChildren<Animator>();
		mainCamera = GetComponentInChildren<Camera>();
		ragdoll = GetComponentInChildren<PirateCharacterAnimator>();
		sceneManager = SceneManager_Andrew.instance;
        startingRotation = transform.rotation;
        startingPosition = transform.position;
        controller = GetComponent<CharacterController>();
        lg = GameObject.FindGameObjectWithTag("LevelGen").GetComponent<LevelGen>();
		debugText = GameObject.Find("DEBUG").GetComponent<Text>(); // Quick and dirty debugging


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

			if (!jumping && controller.isGrounded)
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					//velocity.y = controller.velocity.y;
					jumpVelocity = jumpSpeed;
					jumping = true;
				}
			}
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

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && controller.isGrounded)
        {
            actionJump = true;
        }

        // Touch Controls
        CheckSwipes();
    }

    void CheckSwipes()
    {
		if (Input.touchCount > 0)
		{
			if (!swiping)
			{
				swiping = true;
				Touch tch = Input.GetTouch(0);
				touchDelta = (touchPrevious - tch.position).normalized;

				if (touchDelta.x > deadzone)
				{
					actionRight = true;
				}

				if (touchDelta.x < deadzone)
				{
					actionLeft = true;
				}

				if (touchDelta.y > deadzone && controller.isGrounded)
				{
					actionJump = true;
				}

				touchPrevious = tch.position;

				debugText.text = "TOUCH DEBUGGING\nX: " + touchDelta.x + "\nY: " + touchDelta.y + "";
			}
		}
		else
		{
			swiping = false;
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

					jumpVelocity = jumpSpeed;
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

                    turnTimer += (runSpeed * Time.deltaTime) / 10.0f;

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

		//// Running into sides of track
		//float dist = 1.0f;
		//Vector3 dir = new Vector3(0, 0, 1);

		//Debug.DrawRay(transform.position, dir * dist, Color.green);
		//if (Physics.Raycast(transform.position, dir, dist))
		//{
		//	KillCharacter();
		//}
	}

    void Movement()
    {
		// Debug
		arc.Add(transform.position);

		if (arc.Count > 100)
		{
			arc.RemoveAt(0);
		}

		for (int i = 0; i < arc.Count - 1; i++)
		{
			Color c = arc[i].y < 0 ? Color.red : Color.green;

			Debug.DrawLine(arc[i], arc[i + 1], c);
			Debug.DrawLine(arc[i], arc[i] - transform.up, Color.grey);
		}

		// Camera effects
		mainCamera.fieldOfView = 60.0f + drunkenness / 5.0f;

		Vector3 cameraLean = mainCamera.transform.localEulerAngles;
		cameraLean.z = laneVelocity * drunkenness / 75.0f;
		mainCamera.transform.localEulerAngles = cameraLean;

		// Drunkenness
		if (drunkenness != newDrunkenness)
        {
            drunkTimer += Time.deltaTime / drunkDelay;
            drunkenness = (int)Mathf.Lerp(prevDrunkenness, newDrunkenness, drunkTimer);
        }

        // Lane Hopping
        float laneDelay = Mathf.Lerp(minLaneDelay, maxLaneDelay, drunkenness / 100.0f);

        lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

		Vector3 vel = transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

		controller.Move(vel);

		// Leaning
		transform.Find("Pirate_Character").localEulerAngles = new Vector3(0, 0, -laneVelocity * (1.0f + drunkenness / 20.0f));

		// Running
		runSpeed = Mathf.Lerp(minRunSpeed, maxRunSpeed, drunkenness / 100.0f);

		velocity = transform.forward * runSpeed;

		if (sceneManager != null)
        {
            sceneManager.m_Distance += runSpeed * Time.deltaTime;
        }

        // Jumping
        float jumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, drunkenness / 100.0f);

		/*
		s = jumpHeight
		u = u
		v = 0
		a = gravity
		t = ~

		Equation without t is:

		u^2 = v^2 - 2as

		hence as v = 0 (we want speed to be 0 so that its at the 'top' of the jump)

		u = sqrt(-2as)
		*/

		float gravity = 10.0f * (jumpHeight * 0.85f);

		jumpSpeed = Mathf.Sqrt(2.0f * gravity * jumpHeight);

		velocity.y = jumpVelocity;

		// Falling
		if (!controller.isGrounded)
		{
			jumpVelocity -= gravity * Time.deltaTime;
		}

		controller.Move(velocity * Time.deltaTime);

	}

	void KillCharacter()
	{
		sceneManager.Die();

		mainCamera.transform.SetParent(null);
		controller.enabled = false;
		ragdoll.GoToRagdoll();
		ragdolled = true;

		if (sceneManager.m_Lives <= 0)
		{
			dead = true;
		}
		else
		{
			Invoke("ResetCharacter", 2.0f);
		}
	}

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (sceneManager != null && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && !ragdolled)
        {
			AudioSource.PlayClipAtPoint(smackSound, transform.position);
			KillCharacter();
		}

		if (jumping && !controller.isGrounded)
		{
			if (!ragdolled)
			{
				AudioSource.PlayClipAtPoint(landSound, transform.position);
				anim.Play("Falling");
			}
		}

		jumpVelocity = 0;
		jumping = false;
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
			// Avoid flying
			if (!jumping)
			{
				cornerPoint = collider.gameObject.transform.position;

				playerMode = PlayerMode.TURNING;
			}
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
		mainCamera.transform.SetParent(transform);

		if (ragdolled)
		{
			ragdoll.ResetRagdoll();
			ragdolled = false;
		}

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

		controller.enabled = true;
	}

    public void GetDrunk(int value)
    {
        prevDrunkenness = drunkenness;
        newDrunkenness += value;
        drunkTimer = 0;
    }

    public void SoberUp(int value)
    {
        prevDrunkenness = drunkenness;
        newDrunkenness -= value;
        drunkTimer = 0;
    }
}