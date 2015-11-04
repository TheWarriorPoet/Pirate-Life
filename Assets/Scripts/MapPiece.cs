using UnityEngine;
using System.Collections;

public class MapPiece : MonoBehaviour {
	public GameObject LeftLane, MiddleLane, RightLane;
	public Vector3 LeftLaneOffset = new Vector3(-3,0,0),MiddleLaneOffset = new Vector3(0,0,0), RightLaneOffset = new Vector3(3,0,0);
	// Use this for initialization
	void Start () {
		GameObject obj = (GameObject)GameObject.Instantiate(LeftLane,gameObject.transform.position + (gameObject.transform.rotation * LeftLaneOffset),gameObject.transform.rotation);
		obj.transform.SetParent (gameObject.transform);
		obj = (GameObject)GameObject.Instantiate(MiddleLane,gameObject.transform.position,gameObject.transform.rotation);
		obj.transform.SetParent (gameObject.transform);
		obj = (GameObject)GameObject.Instantiate(RightLane,gameObject.transform.position + (gameObject.transform.rotation * RightLaneOffset),gameObject.transform.rotation);
		obj.transform.SetParent (gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
