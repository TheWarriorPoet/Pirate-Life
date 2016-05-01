using UnityEngine;
using System.Collections;

public class SmoothCam : MonoBehaviour
{
	public float rotationSpeed;
	public float positionSpeed;

	Player player;
	Vector3 posOffset;
	Quaternion rotOffset;
	Camera cam;
	Vector3 targetPos;
	Quaternion targetRot;

	void Start()
	{
		cam = GetComponent<Camera>();
		player = FindObjectOfType<Player>();

		posOffset = transform.position - player.transform.position;
		rotOffset = transform.rotation;

		transform.parent = null;
	}

	void FixedUpdate()
	{
		// Position
		targetPos = player.transform.position;

		// Rotate
		Vector3 rot = transform.rotation.eulerAngles;
		float yRot = player.transform.rotation.eulerAngles.y;
		rot.y = yRot;
		targetRot = Quaternion.Euler(rot);

		// Offset
		targetPos += posOffset.z * transform.forward;
		targetPos += posOffset.y * transform.up;

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
		Debug.DrawLine(player.transform.position, targetPos);
	}
}
