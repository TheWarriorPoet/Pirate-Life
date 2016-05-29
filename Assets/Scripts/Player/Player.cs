using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public enum PlayerMode
    {
        RUNNING,
        TURNING,
		SLIPPING,
        CRASHING
    }

	[Header("Player Settings")]
	public PlayerMode playerMode;
    public int drunkenness;
	public int drunkSmashValue;
	public float drunkDelay;
    public float minRunSpeed, maxRunSpeed;
    public float minJumpHeight, maxJumpHeight;
    public float minLaneDelay, maxLaneDelay;
    private float prevMultiplier = 1.0f;
	public float multiplier;
	public float laneDistance;
	public float slipDuration;
	public int currentLane;
	public bool jumping, isTurning;
	[Header("Touch Settings")]
	public float angleDeadzone;
	public float swipeDeadzone;
	public float swipeLength;
	[Header("Object Linking")]
	public ProcGen lg;
	public GameObject splashPrefab, smashPrefab, drunkEffect, soberEffect;
	[Header("Debugging")]
	public float runSpeed;
	public Vector3 velocity;
	public bool isGrounded;

	private SmoothCam mainCamera;
	private Animator anim;
	private ParticleSystem drunkParticles, soberParticles;
	private PirateCharacterAnimator ragdoll;
	private SceneManager_Andrew sceneManager = null;
    private Vector3 startingPosition = Vector3.zero;
    private Quaternion startingRotation = Quaternion.identity;
    private CharacterController controller;
    private bool dead = false, ragdolled = false;
    private AudioClip jumpSound, deckSound, landSound, splashSound, smackSound, skidSound;
    // Controls
    private bool actionLeft, actionRight, actionJump;
	private float jumpVelocity, jumpSpeed;
	List<Vector3> arc = new List<Vector3>();
	// Drunkenness
	private int prevDrunkenness;
	private int newDrunkenness;
    private float drunkTimer;
	// Slipping
	private float slipTimer;
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
	Vector2 touchDelta, touchPrevious, touchTotal;
	private bool swiped;
	//Text debugText; // Quick and dirty debugging
    // Player Mesh
    private bool _parrotActive = false;

    private GameManager _gameManager = null;

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
        _gameManager = GameManager.instance;
        if(_gameManager != null)
        {
            foreach (UpgradeStruct us in _gameManager._allUpgrades)
            {
                if (us.upgradeValues[0].upgradeType == UpgradeType.ParrotUpgrade)
                {
                    if (us.Purchased)
                    {
                        _parrotActive = true;
                    }
                    break;
                }
            }
        }
        anim = GetComponentInChildren<Animator>();
		mainCamera = FindObjectOfType<SmoothCam>();

		// Effects
		Vector3 offset = transform.up * 2 + transform.forward * 0.35f;
		GameObject drunkObj = (GameObject)Instantiate(drunkEffect, transform.position + offset, transform.rotation);
		drunkObj.transform.parent = transform;
		drunkParticles = drunkObj.GetComponent<ParticleSystem>();
		GameObject soberObj = (GameObject)Instantiate(soberEffect, transform.position + offset, transform.rotation);
		soberObj.transform.parent = transform;
		soberParticles = soberObj.GetComponent<ParticleSystem>();

		ragdoll = GetComponentInChildren<PirateCharacterAnimator>();
		sceneManager = SceneManager_Andrew.instance;
        startingRotation = transform.rotation;
        startingPosition = transform.position;
        controller = GetComponent<CharacterController>();
		lg = GameObject.FindGameObjectWithTag("LevelGen").GetComponent<ProcGen>();
		//debugText = GameObject.Find("DEBUG").GetComponent<Text>(); // Quick and dirty debugging

		jumpSound = (AudioClip)Resources.Load("Sounds/player_jump");
        splashSound = (AudioClip)Resources.Load("Sounds/player_splash");
		smackSound = (AudioClip)Resources.Load("Sounds/player_smack");
		deckSound = (AudioClip)Resources.Load("Sounds/deck_jump");
		landSound = (AudioClip)Resources.Load("Sounds/player_land");
		skidSound = (AudioClip)Resources.Load("Sounds/player_skid");

		ResetCharacter();
    }

    void Update()
    {
		if (Time.timeScale == 0) return;

        if (!dead && !ragdolled)
        {
            Controls();
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
		if (!ragdolled)
		{
			if (!dead)
			{
				Action();
			}
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

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && controller.isGrounded)
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
			//debugText.text = "TOUCH DEBUGGING";

			if (!swiped)
			{
				Touch tch = Input.GetTouch(0);
				touchDelta = touchPrevious - tch.position;

				//debugText.text += "\nLength: " + touchDelta.magnitude + "\nDeadzone: " + (Screen.width * swipeDeadzone);

				if (touchDelta.magnitude > (Screen.width * swipeDeadzone))
				{
					touchDelta.Normalize();

					// Deadzone
					if (Mathf.Abs(touchDelta.x) > angleDeadzone)
					{
						touchTotal.x += touchDelta.x;
					}
					else if (Mathf.Abs(touchDelta.y) > angleDeadzone)
					{
						touchTotal.y += touchDelta.y;
					}

					// Actions
					if (touchTotal.x > swipeLength)
					{
						actionLeft = true;
						ResetTouchData();
					}

					if (touchTotal.x < -swipeLength)
					{
						actionRight = true;
						ResetTouchData();
					}

					if (touchTotal.y < -swipeLength)
					{
						if (controller.isGrounded)
						{
							actionJump = true;
						}
						ResetTouchData();
					}
				}

				touchPrevious = tch.position;
			}

			//debugText.text += "\nX: " + touchTotal.x + "\nY: " + touchTotal.y;
		}
		else
		{
			ResetTouchData();
			swiped = false;
		}
	}

	void ResetTouchData()
	{
		swiped = true;
		touchTotal = Vector2.zero;
		touchDelta = Vector2.zero;
		touchPrevious = Vector2.zero;
	}

    void Action()
    {
        switch (playerMode)
        {
			case PlayerMode.CRASHING:
			case PlayerMode.RUNNING:
				if (anim.speed > 1) anim.speed = 1;
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
                break;

            case PlayerMode.TURNING:
				if (anim.speed > 1) anim.speed = 1;
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
					Vector3 pos = Vector3.Lerp(point1, point2, turnTimer);
					pos.y = transform.position.y;

					transform.position = pos;

                    // Rotation
                    //turnAngle = Mathf.LerpAngle(transform.rotation.y, turnDegree, turnTimer);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, turnDegree, 0), turnTimer);

                    // Stop Turning
                    if (turnTimer >= 1)
                    {
                        currentLane = 0;
                        laneVelocity = 0;
                        lanePosition = 0;

						//isTurning = false;
						playerMode = PlayerMode.RUNNING;
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
			case PlayerMode.SLIPPING:

				if (anim.speed < 4) anim.speed = 4;
				slipTimer += Time.deltaTime;
				if (slipTimer >= slipDuration)
				{
					anim.speed = 1;
					slipTimer = 0;
					playerMode = PlayerMode.RUNNING;
				}
				break;
		}

		// Can always jump unless ragdolled
		if (actionJump && !jumping)
		{
			AudioSource.PlayClipAtPoint(jumpSound, transform.position);
			AudioSource.PlayClipAtPoint(deckSound, transform.position);
			anim.Play("Jumping");

			jumpVelocity = jumpSpeed;
			jumping = true;
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

		isGrounded = controller.isGrounded;
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

		// Drunkenness
		if (drunkenness != newDrunkenness)
        {
            drunkTimer += Time.deltaTime / drunkDelay;
            drunkenness = (int)Mathf.Lerp(prevDrunkenness, newDrunkenness, drunkTimer);
        }
		else
		{
			StopParticles();
		}

		if (!dead)
		{
			// Lane Hopping
			float laneDelay = Mathf.Lerp(minLaneDelay, maxLaneDelay, drunkenness / 100.0f);

			lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

			Vector3 vel = transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

			controller.Move(vel);

			// Leaning
			anim.gameObject.transform.localEulerAngles = new Vector3(0, 0, -laneVelocity * (1.0f + drunkenness / 20.0f));

			// Running
			float baseRunSpeed = Mathf.Lerp(minRunSpeed, maxRunSpeed, drunkenness / 100.0f);
			runSpeed = baseRunSpeed * multiplier;

			velocity = transform.forward * runSpeed;

			if (sceneManager != null)
			{
				sceneManager.m_Distance += runSpeed * Time.deltaTime;
			}
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

	void KillCharacter(bool causeRagdoll)
	{
		sceneManager.Die();

		mainCamera.followPlayer = false;

		if (causeRagdoll)
		{
			controller.enabled = false;
			ragdoll.GoToRagdoll();
			ragdolled = true;
		}

		StopParticles();

		if (sceneManager.m_Lives <= 0)
		{
			dead = true;
		}
		else
		{
			Invoke("ResetCharacter", 2.0f);
		}
	}


    IEnumerator CrateCrashing()
    {
        float timer = 0.0f;
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in allRenderers)
        {
			if (r)
			{
				if (r.material.HasProperty("_Color"))
				{
					Vector4 alpha = r.material.color;
					alpha.w = 0.5f;
					r.material.color = alpha;
				}
			}
		}
        for (int i = 0; i < 8; ++i) {
            while (timer < 0.2f)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0.0f;
            foreach (Renderer r in allRenderers)
            {
				if (r)
				{
					if (r.gameObject.tag != "Parrot")
						r.enabled = !r.enabled;
					else if (r.gameObject.tag == "Parrot" && _parrotActive)
						r.enabled = !r.enabled;
				}
            }
        }
        foreach (Renderer r in allRenderers)
        {
			if (r)
			{
				if (r.material.HasProperty("_Color"))
				{
					Vector4 alpha = r.material.color;
					alpha.w = 1.0f;
					r.material.color = alpha;
				}
				r.enabled = true;
			}
        }
		if (!_parrotActive)
		{
			UpgradeManager.instance.DeactivateParrot();
		}

        multiplier = prevMultiplier;
        if (playerMode == PlayerMode.CRASHING)
            playerMode = PlayerMode.RUNNING;
        yield return null;
    }

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (sceneManager != null && collision.gameObject.CompareTag("Smashable") && !ragdolled)
        {
            if (playerMode == PlayerMode.CRASHING)
            {
                return;
            }
            else if (drunkenness >= drunkSmashValue)
            {
                foreach (GameObject go in GetComponentInChildren<NearbyCrates>().CratesAhead)
                {
                    go.SetActive(false);
                }

                AudioSource.PlayClipAtPoint(smackSound, transform.position);
                SoberUp(drunkSmashValue);
				Instantiate(smashPrefab, transform.position + (transform.forward * 3.0f), transform.rotation);
                prevMultiplier = multiplier;
                multiplier = 1.0f;
                playerMode = PlayerMode.CRASHING;
				StartCoroutine("CrateCrashing");
            }
		}

		if (sceneManager != null && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
		{
			if (collision.gameObject.CompareTag("Smashable") && playerMode == PlayerMode.CRASHING)
			{
				return;
			}

			AudioSource.PlayClipAtPoint(smackSound, transform.position);
			KillCharacter(true);
		}

		if (jumping && !controller.isGrounded && collision.moveDirection.y < 0)
		{
			if (!ragdolled)
			{
				AudioSource.PlayClipAtPoint(landSound, transform.position);
				anim.Play("Falling");
				jumping = false;
			}
		}

		if (collision.moveDirection.y < 0 && !jumping)
		{
			jumpVelocity = -0.1f;
		}
	}

    void OnTriggerEnter(Collider collider)
    {
		if (collider.gameObject.layer == LayerMask.NameToLayer("GameWater"))
		{
			Vector3 offset = new Vector3(0, 1, 0);
			Instantiate(splashPrefab, transform.position + offset, transform.rotation);

			Invoke("StopDeathCam", 0.25f);

			AudioSource.PlayClipAtPoint(splashSound, transform.position);
			velocity = Vector3.zero;
			KillCharacter(false);
		}

		if (collider.gameObject.layer == LayerMask.NameToLayer("Hazard"))
		{
			AudioSource.PlayClipAtPoint(skidSound, transform.position);
			playerMode = PlayerMode.SLIPPING;
			Debug.Log("Slipped!");
		}

		if (collider.gameObject.layer == LayerMask.NameToLayer("CornerTrigger"))
		{
			// Avoid flying
			//if (!jumping)
			//{
				cornerPoint = collider.gameObject.transform.position;
				cornerPoint.y = transform.position.y;

				playerMode = PlayerMode.TURNING;
			//}
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
		if (ragdolled)
		{
			ragdoll.ResetRagdoll();
			ragdolled = false;
		}

		mainCamera.followPlayer = true;
		mainCamera.deathCam = true;

        transform.position = startingPosition;
        transform.rotation = startingRotation;

		currentLane = 0;
        laneVelocity = 0;
        lanePosition = 0;

        cornerStart = Vector3.zero;
        cornerPoint = Vector3.zero;
        cornerEnd = Vector3.zero;
        turnTimer = 0;
        turnDegree = 0;

        jumping = false;
		jumpVelocity = 0.0f;

        drunkenness = 0;
        drunkTimer = 0;
		slipTimer = 0;
        prevDrunkenness = 0;
        newDrunkenness = 0;

		multiplier = 1.0f;

		lg.RebuildMap();

		anim.Play("Movement");

		controller.enabled = true;

        _gameManager.PlayerReset = true;

		playerMode = PlayerMode.RUNNING;

        ResetTouchData();
		swiped = false;

		mainCamera.ResetCam();
	}

	void StopDeathCam()
	{
		mainCamera.deathCam = false;
	}

	void StopParticles()
	{
		if (drunkParticles.isPlaying)
		{
			drunkParticles.Stop();
		}

		if (soberParticles.isPlaying)
		{
			soberParticles.Stop();
		}
	}

    public void GetDrunk(int value)
    {
		drunkParticles.Play();

		prevDrunkenness = drunkenness;
        newDrunkenness += value;
        drunkTimer = 0;
    }

    public void SoberUp(int value)
    {
		soberParticles.Play();

		prevDrunkenness = drunkenness;
        newDrunkenness -= value;
        drunkTimer = 0;
    }

	public float GetLaneVelocity()
	{
		return laneVelocity;
	}
}