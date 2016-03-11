using UnityEngine;
using System.Collections;

public class SkyboxPositioner : MonoBehaviour
{
	Camera cam;

	void Start()
	{
		cam = FindObjectOfType<Camera>();
	}

	void Update()
	{
		Vector3 pos = transform.position;

		pos.x = cam.transform.position.x;
		pos.z = cam.transform.position.z;

		transform.position = pos;
	}
}
