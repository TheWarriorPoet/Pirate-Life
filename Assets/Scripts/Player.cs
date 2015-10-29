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
		Vector3 pos = transform.position;

		// Lane Hopping
		float laneDelay = Mathf.Lerp(minLaneDelay, maxLaneDelay, drunkenness / 100);

		lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

		pos += transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

		// Running
		float runSpeed = Mathf.Lerp(minRunSpeed, maxRunSpeed, drunkenness / 100);

		pos += (transform.forward * runSpeed) * Time.deltaTime;

		// Jumping
		if (jumping)
		{
			pos += (transform.up * minJumpHeight) * Time.deltaTime;
		}

		transform.position = pos;
	}

    void OnCollisionEnter(Collision a_Collision)
    {
        if (a_Collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _mySceneManager.Die();
            ResetCharacter();
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