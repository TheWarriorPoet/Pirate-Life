﻿using System.Collections;
using UnityEngine;

public class CornerCollider : MonoBehaviour {
	public GameObject Player;
	public GameObject WorldContainer;
	bool hasRotated = false;
	bool hasExited = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider Other)
	{
		if (Other.gameObject == Player) {
			hasExited = false;
		}
	}

	void OnTriggerStay(Collider Other)
	{
		if (Other.gameObject == Player) {
			if(!hasRotated && !hasExited)
			{
				if(Input.GetAxis("Horizontal") < 0)
				{
					Player.transform.Rotate(new Vector3(0,-90,0));
					hasRotated = true;
				}else if(Input.GetAxis ("Horizontal") > 0)
				{
					Player.transform.Rotate(new Vector3(0,90,0));
					hasRotated = true;
				}
			}
		}
	}
	void OnTriggerExit(Collider Other)
	{
		if (Other.gameObject == Player) {
			hasExited = true;
			hasRotated = false;
		}
	}
}
