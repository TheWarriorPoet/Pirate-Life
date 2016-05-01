using UnityEngine;
using System.Collections;

public class SmoothCam : MonoBehaviour
{
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
		transform.position = player.transform.position + posOffset;

		// Camera effects
		cam.fieldOfView = 60.0f + player.drunkenness / 5.0f;

		Vector3 cameraLean = cam.transform.localEulerAngles;
		cameraLean.z = player.GetLaneVelocity() * player.drunkenness / 75.0f;
		cam.transform.localEulerAngles = cameraLean;
	}
}
