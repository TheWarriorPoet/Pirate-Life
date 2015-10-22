using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float runSpeed;
	public float jumpHeight;
	public float laneDelay;
	public float laneDistance;
	public int currentLane;

	private SceneManager_Andrew _mySceneManager = null;
    private Transform _PlayerTransform = null;
    private Vector3 _StartingPosition = Vector3.zero;
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
	}

	void Movement()
	{
		Vector3 pos = transform.position;

		// Lane Hopping
		lanePosition = Mathf.SmoothDamp(lanePosition, currentLane, ref laneVelocity, laneDelay);

		pos += transform.right * (laneVelocity * laneDistance) * Time.deltaTime;

		// Running
		pos += (transform.forward * runSpeed) * Time.deltaTime;

		// Jumping
		if (jumping)
		{
			pos += (transform.up * jumpHeight) * Time.deltaTime;
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
        _PlayerTransform.position = _StartingPosition;
    }
}