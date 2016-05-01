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

	void Start()
	{
		cam = GetComponent<Camera>();
		player = FindObjectOfType<Player>();

		posOffset = transform.position - player.transform.position;
		rotOffset = transform.rotation;

		transform.parent = null;
	}

	void Update()
	{
		// Position
		transform.position = player.transform.position;

		// Rotate
		Vector3 rot = transform.rotation.eulerAngles;
		float yRot = player.transform.rotation.eulerAngles.y;
		rot.y = yRot;
		transform.rotation = Quaternion.Euler(rot);

		// Offset
		transform.position += posOffset.z * transform.forward;
		transform.position += posOffset.y * transform.up;

		// Camera effects
		cam.fieldOfView = 60.0f + player.drunkenness / 5.0f;

		Vector3 cameraLean = cam.transform.localEulerAngles;
		cameraLean.z = player.GetLaneVelocity() * player.drunkenness / 75.0f;
		cam.transform.localEulerAngles = cameraLean;
	}
}
