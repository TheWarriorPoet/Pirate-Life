using UnityEngine;
using System.Collections;

public class Seagull : MonoBehaviour
{
	public float speed, angle, smoothing, flightHeight, heightTime, maxRadius;
	float rot, rotVel, height, startHeight, heightVel, heightTimer, targetHeight;
	Camera cam;

	void Start()
	{
		startHeight = transform.position.y;
		height = startHeight;
		heightTimer = Random.Range(0.0f, heightTime);
		cam = FindObjectOfType<Camera>();

		targetHeight = Random.Range(startHeight, startHeight + flightHeight); // repeat ew
	}

	void Update()
	{
		// Height of flying
		heightTimer += Time.deltaTime;

		if (heightTimer >= heightTime)
		{
			heightTimer = 0;
			targetHeight = Random.Range(startHeight, startHeight + flightHeight);
		}
		height = Mathf.SmoothDamp(height, targetHeight, ref heightVel, 1.0f / heightTimer);

		// Heading direction
		float targetRot = Random.Range(-angle, angle);
		rot = Mathf.SmoothDampAngle(rot, targetRot, ref rotVel, smoothing);

		// Move
		transform.Rotate(0, rot, 0);
		transform.position += transform.forward * speed * Time.deltaTime;

		Vector3 pos = transform.position;
		pos.y = height;
		transform.position = pos;

		// Clamp horizontal position to radius
		ClampToRadius();
	}

	void ClampToRadius()
	{
		float dst = Vector3.Distance(cam.transform.position, transform.position);

		if (dst > maxRadius)
		{
			Vector3 vect = cam.transform.position - transform.position;
			vect = vect.normalized;
			vect *= (dst - maxRadius);
			vect.y = 0;
			transform.position += vect;
		}
	}
}