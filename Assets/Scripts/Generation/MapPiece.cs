using UnityEngine;
using System.Collections;

public class MapPiece : MonoBehaviour {
	public GameObject LeftLane, MiddleLane, RightLane;
	public Vector3 LeftLaneOffset = new Vector3(-3,0,0),MiddleLaneOffset = new Vector3(0,0,0), RightLaneOffset = new Vector3(3,0,0);
	private GameObject objL, objM, objR;
	// Use this for initialization
	void Start () {
		objL = (GameObject)GameObject.Instantiate(LeftLane,gameObject.transform.position + (gameObject.transform.rotation * LeftLaneOffset),gameObject.transform.rotation);
		objL.transform.SetParent (gameObject.transform);
		objM = (GameObject)GameObject.Instantiate(MiddleLane,gameObject.transform.position + MiddleLaneOffset,gameObject.transform.rotation);
		objM.transform.SetParent (gameObject.transform);
		objR = (GameObject)GameObject.Instantiate(RightLane,gameObject.transform.position + (gameObject.transform.rotation * RightLaneOffset),gameObject.transform.rotation);
		objR.transform.SetParent (gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BuildPiece()
	{
		Destroy (objL);
		Destroy (objM);
		Destroy (objR);
		objL = (GameObject)GameObject.Instantiate(LeftLane,gameObject.transform.position + (gameObject.transform.rotation * LeftLaneOffset),gameObject.transform.rotation);
		objL.transform.SetParent (gameObject.transform);
		objM = (GameObject)GameObject.Instantiate(MiddleLane,gameObject.transform.position + MiddleLaneOffset,gameObject.transform.rotation);
		objM.transform.SetParent (gameObject.transform);
		objR = (GameObject)GameObject.Instantiate(RightLane,gameObject.transform.position + (gameObject.transform.rotation * RightLaneOffset),gameObject.transform.rotation);
		objR.transform.SetParent (gameObject.transform);
	}

}
