using UnityEngine;
using System.Collections.Generic;

public class MapPiece : MonoBehaviour {
	public List<GameObject> Lanes;
	public List<Vector3> LaneOffsets;
	public List<GameObject> Collectables;
	public List<GameObject> Obstacles;
	private List<GameObject> objs = new List<GameObject> ();
	// Use this for initialization
	void Start () {

		foreach (GameObject obj in objs) {
			Destroy (obj);
		}

		objs.Clear ();
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
