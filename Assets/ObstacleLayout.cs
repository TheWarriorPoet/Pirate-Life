using UnityEngine;
using System.Collections.Generic;


public class ObstacleLayout : MonoBehaviour {


	public List<GameObject> Obstacles = new List<GameObject>();
	public List<Vector3> ObstaclesOffset = new List<Vector3>();

	private List<GameObject> Colls = new List<GameObject>(11);
	// Use this for initialization
	void Start () {
		RebuildObstacles ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void RebuildObstacles()
	{
		Colls.Clear ();
		for (int i = 0; i < Obstacles.Count; ++i) {
			GameObject obj = (GameObject)GameObject.Instantiate(Obstacles[i],(transform.rotation * ObstaclesOffset[i]) + transform.position,gameObject.transform.rotation);
			obj.transform.SetParent(gameObject.transform);
			Colls.Add (obj);
		}
	}
}
