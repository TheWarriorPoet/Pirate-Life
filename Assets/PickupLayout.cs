using UnityEngine;
using System.Collections.Generic;


public class PickupLayout : MonoBehaviour {


	public List<GameObject> Collectables;
	public List<Vector3> CollectablesOffset;

	private List<GameObject> Colls = new List<GameObject>(11);
	// Use this for initialization
	void Start () {
		RebuildCollectables ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void RebuildCollectables()
	{
		Colls.Clear ();
		for (int i = 0; i < Collectables.Count; ++i) {
			GameObject obj = (GameObject)GameObject.Instantiate(Collectables[i],(transform.rotation * CollectablesOffset[i]) + transform.position,gameObject.transform.rotation);
			obj.transform.SetParent(gameObject.transform);
			Colls.Add (obj);
		}
	}
}
