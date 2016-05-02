using UnityEngine;
using System.Collections;

public class ShipMove : MonoBehaviour {
	public float Speed;
	public Vector3 RotationAngle;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (RotationAngle * Time.deltaTime);
		transform.position += (transform.forward * (Speed * Time.deltaTime));
	}
}
