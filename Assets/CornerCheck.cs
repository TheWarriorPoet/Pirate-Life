using UnityEngine;
using System.Collections;

public class CornerCheck : MonoBehaviour {

	public bool hasTurned = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider Other)
	{
		if (Other.tag == "Player") {
			//Can no longer change lanes
		}
	}

	void OnTriggerExit(Collider Other)
	{
		if (Other.tag == "Player") {
			//Can now change lanes
		}
	}
}
