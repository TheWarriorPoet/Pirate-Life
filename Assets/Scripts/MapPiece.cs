using UnityEngine;
using System.Collections;

public class MapPiece : MonoBehaviour {
	public GameObject LeftLane, MiddleLane, RightLane;
	public Vector3 LeftLaneOffset = new Vector3(-3,0,0),MiddleLaneOffset = new Vector3(0,0,0), RightLaneOffset = new Vector3(3,0,0);
	// Use this for initialization
	void Start () {
		GameObject obj = (GameObject)GameObject.Instantiate(LeftLane,gameObject.transform.position + LeftLaneOffset,Quaternion.identity);
		obj.transform.SetParent (gameObject.transform);
		obj = (GameObject)GameObject.Instantiate(MiddleLane,gameObject.transform.position,Quaternion.identity);
		obj.transform.SetParent (gameObject.transform);
		obj = (GameObject)GameObject.Instantiate(RightLane,gameObject.transform.position + RightLaneOffset,Quaternion.identity);
		obj.transform.SetParent (gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
