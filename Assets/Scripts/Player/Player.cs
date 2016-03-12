using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	public float gravity = 9.8f;
	public int currentLane;
	public bool jumping, falling, grounded;
	public bool isTurning;
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
    // Drunkenness
    private float prevDrunkenness;
    private float newDrunkenness;
    private float drunkTimer;
    // Lane Switching
    private float laneVelocity;
    private float lanePosition;
    private int previousLane;
	// Jumping
	private float jumpDistance;
    // Corner Turning
    private Vector3 cornerStart;
    private Vector3 cornerPoint;
    private Vector3 cornerEnd;
    private float turnTimer;
    private float turnDegree;
	// Touch
	Vector2 touchDelta, touchPrevious;
    private bool swiping;
	private float deadzone = 0.8f;
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

                if (actionJump && !jumping && !falling)
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
		Vector3 vel = Vector3.zero;

		// Camera effects
		mainCamera.fieldOfView = 60.0f + drunkenness / 5.0f;
		mainCamera.transform.localEulerAngles = new Vector3(15, 0, laneVelocity * drunkenness / 75.0f);

		// Falling
		float dist = 1.0f;
		Vector3 dir = new Vector3(0, -1, 0);

		Debug.DrawRay(transform.position, dir * dist, Color.green);
		if (!Physics.Raycast(transform.position, dir, dist))
		{
			if (!jumping)
			{
				falling = true;
			}
		}

        // Drunkenness
        if (drunkenness != newDrunkenness)
        {
            drunkTimer += Time.deltaTime / drunkDelay;
            drunkenness = (int)Mathf.Lerp(prevDrunkenness, newDrunkenness, drunkTimer);
        }

        // Lane Hopping
        float laneDelay = Mathf.Lerp(minLaneDelay, maxLaneDelay, drunkenness / 100.0f);

        lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

		vel += transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

        // Leaning
        transform.Find("Pirate_Character").localEulerAngles = new Vector3(0, 0, -laneVelocity * (1.0f + drunkenness / 20.0f));

		// Running
		float runSpeed = Mathf.Lerp(minRunSpeed, maxRunSpeed, drunkenness / 100.0f);

		vel += (transform.forward * runSpeed) * Time.deltaTime;

        if (sceneManager != null)
        {
            sceneManager.m_Distance += runSpeed;
        }

        // Jumping
        float jumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, drunkenness / 100.0f);

        if (jumping)
        {
			float jump = gravity * Time.deltaTime;

			jumpDistance += jump;

			if (jumpDistance > jumpHeight)
			{
				jumping = false;
				jumpDistance = 0;
			}
			else
			{
				vel.y += jump;
			}

			Debug.Log(jumpDistance);
        }

		// Falling
		if (falling)
		{
			vel.y -= gravity * Time.deltaTime;
		}

		controller.Move(vel);
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

		if (falling)
		{
			if (!ragdolled)
			{
				AudioSource.PlayClipAtPoint(landSound, transform.position);
				anim.Play("Falling");
			}
			falling = false;
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
			// Avoid flying
			if (jumping)
			{
				AudioSource.PlayClipAtPoint(landSound, transform.position);
				anim.Play("Falling");
				jumping = false;
			}

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
		falling = false;
		grounded = false;

        drunkenness = 0;
        drunkTimer = 0;
        prevDrunkenness = 0;
        newDrunkenness = 0;

        lg.RebuildMap();

		anim.Play("Movement");

		controller.enabled = true;
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