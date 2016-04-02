using UnityEngine;
using System.Collections;

public class Seagull : MonoBehaviour
{
	public float speed, angle, turnSmoothing;
	float rot, rotVel;

	void Start()
	{

	}

	void Update()
	{
		float targetRot = Random.Range(-angle, angle);
		rot = Mathf.SmoothDampAngle(rot, targetRot, ref rotVel, turnSmoothing);

		transform.Rotate(0, rot, 0);
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}