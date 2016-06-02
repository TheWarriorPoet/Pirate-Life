using UnityEngine;
using System.Collections;

public class SmoothCam : MonoBehaviour
{
	public float rotationSpeed;
	public float positionSpeed;
	public bool followPlayer;
	public GameObject deathTarget;
	public bool deathCam;

	Player player;
	Vector3 posOffset;
	Quaternion rotOffset;
	Camera cam;
	Vector3 targetPos;
	Quaternion targetRot;

	void Start()
	{
		followPlayer = true;
		cam = GetComponent<Camera>();
		player = GetComponentInParent<Player>();

		transform.parent = null;

		posOffset = transform.position - player.transform.position;
		rotOffset = transform.rotation;
	}

	void Update()
	{
		// Debug
		if (Input.GetKeyDown(KeyCode.C))
		{
			ResetCam();
		}
	}

	void FixedUpdate()
	{
		if (followPlayer)
		{
			CalculateTarget();

			FollowTarget();
		}
		else if (deathCam)
		{
			TrackDeath();
		}
	}

	void FollowTarget()
	{
		// Move smoothly to target
		transform.position = Vector3.Lerp(transform.position, targetPos, positionSpeed * Time.deltaTime);
		//transform.position = targetPos;
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
		//transform.rotation = targetRot;

		// Camera effects
		cam.fieldOfView = 60.0f + player.drunkenness / 5.0f;

		Vector3 cameraLean = cam.transform.localEulerAngles;
		cameraLean.z = player.GetLaneVelocity() * player.drunkenness / 75.0f;
		cam.transform.localEulerAngles = cameraLean;

		// Debug
		Debug.DrawLine(player.transform.position, targetPos, Color.yellow);
		Debug.DrawLine(targetPos, transform.position, Color.magenta);
	}

	void TrackDeath()
	{
		if (deathTarget == null)
		{
			return;
		}

		targetRot = Quaternion.LookRotation(deathTarget.transform.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
	}

	void CalculateTarget()
	{
		if (player == null) return;

		// Position
		targetPos = player.transform.position;

		// Rotate
		Vector3 rot = transform.rotation.eulerAngles;
		float yRot = player.transform.rotation.eulerAngles.y;
		rot.y = yRot;
		rot.x = rotOffset.eulerAngles.x;
		targetRot = Quaternion.Euler(rot);

		// Offset
		targetPos += posOffset.z * player.transform.forward;
		targetPos += posOffset.y * transform.up;
	}

	public void ResetCam()
	{
		if (player == null) return;

		CalculateTarget();

		transform.position = player.transform.position + posOffset;
		transform.rotation = rotOffset;
	}
}
