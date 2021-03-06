﻿using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public Player player;
	public ProcGen pG;
	private bool hasReached = false;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		pG = GameObject.FindGameObjectWithTag ("LevelGen").GetComponent<ProcGen> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider Other)
	{
		if (Other.gameObject.tag == "Player") {
			if (!hasReached) {
				pG.TransitionSections ();
				hasReached = true;
			}
		}
	}
}
