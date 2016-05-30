using UnityEngine;
using System.Collections.Generic;

public class MapPiece : MonoBehaviour {
	public List<GameObject> Lanes;
	public List<Vector3> LaneOffsets;
	public List<GameObject> Collectables;
	public List<GameObject> Obstacles;
	public GameObject Posts;
	private List<GameObject> objs = new List<GameObject> ();
	// Use this for initialization
	void Start () {

		BuildPiece ();
		//GetComponent<PickupLayout> ().RebuildCollectables ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BuildPiece()
	{

		foreach (GameObject obj in objs) {
			Destroy (obj);
		}

		objs.Clear ();
		if (Lanes [0].tag != "Empty") {
			objs.Add ((GameObject)GameObject.Instantiate (Posts, gameObject.transform.position + (gameObject.transform.rotation * (LaneOffsets [0] - new Vector3(1.5f,0,0))), gameObject.transform.rotation));
		} else if (Lanes [1].tag != "Empty") {
			objs.Add ((GameObject)GameObject.Instantiate (Posts, gameObject.transform.position + (gameObject.transform.rotation * (LaneOffsets [1] - new Vector3(1.5f,0,0))), gameObject.transform.rotation));
		} else if (Lanes [2].tag != "Empty") {
			objs.Add ((GameObject)GameObject.Instantiate (Posts, gameObject.transform.position + (gameObject.transform.rotation * (LaneOffsets [2] - new Vector3(1.5f,0,0))), gameObject.transform.rotation));
		}

		if (Lanes [2].tag != "Empty") {
			objs.Add ((GameObject)GameObject.Instantiate (Posts, gameObject.transform.position + (gameObject.transform.rotation * (LaneOffsets [2] + new Vector3(1.5f,0,0))), gameObject.transform.rotation));
		} else if (Lanes [1].tag != "Empty") {
			objs.Add ((GameObject)GameObject.Instantiate (Posts, gameObject.transform.position + (gameObject.transform.rotation * (LaneOffsets [1] + new Vector3(1.5f,0,0))), gameObject.transform.rotation));
		} else if (Lanes [0].tag != "Empty") {
			objs.Add ((GameObject)GameObject.Instantiate (Posts, gameObject.transform.position + (gameObject.transform.rotation * (LaneOffsets [0] + new Vector3(1.5f,0,0))), gameObject.transform.rotation));
		}


		objs.Add((GameObject)GameObject.Instantiate(Lanes[0],gameObject.transform.position + (gameObject.transform.rotation * LaneOffsets[0]),gameObject.transform.rotation));
		objs.Add((GameObject)GameObject.Instantiate(Lanes[1],gameObject.transform.position + LaneOffsets[1],gameObject.transform.rotation));
		objs.Add((GameObject)GameObject.Instantiate(Lanes[2],gameObject.transform.position + (gameObject.transform.rotation * LaneOffsets[2]),gameObject.transform.rotation));

		objs.Add((GameObject)GameObject.Instantiate(Collectables[0],gameObject.transform.position + (gameObject.transform.rotation * LaneOffsets[0]),gameObject.transform.rotation));
		objs.Add((GameObject)GameObject.Instantiate(Collectables[1],gameObject.transform.position + LaneOffsets[1],gameObject.transform.rotation));
		objs.Add((GameObject)GameObject.Instantiate(Collectables[2],gameObject.transform.position + (gameObject.transform.rotation * LaneOffsets[2]),gameObject.transform.rotation));

		objs.Add((GameObject)GameObject.Instantiate(Obstacles[0],gameObject.transform.position + (gameObject.transform.rotation * LaneOffsets[0]),gameObject.transform.rotation));
		objs.Add((GameObject)GameObject.Instantiate(Obstacles[1],gameObject.transform.position + LaneOffsets[1],gameObject.transform.rotation));
		objs.Add((GameObject)GameObject.Instantiate(Obstacles[2],gameObject.transform.position + (gameObject.transform.rotation * LaneOffsets[2]),gameObject.transform.rotation));

		foreach (GameObject obj in objs) {
			obj.transform.SetParent (gameObject.transform);
		}
	}

}
