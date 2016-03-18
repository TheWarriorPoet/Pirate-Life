﻿using UnityEngine;
using System.Collections.Generic;


public class LevelGen : MonoBehaviour {
	public int TrackDirection;
	public float PieceLength = 50;
	//public GameObject CornerBlock; //This is to check if the block is a corner, and to start layering the track around the corner
	public List<GameObject> SectionList;
	private List<GameObject> objList = new List<GameObject>();
	private Vector3 TrackPos = new Vector3(0,0,0);
	// Use this for initialization
	void Start () {
		RebuildMap ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void RebuildMap()
	{
		foreach (GameObject obj in objList) {
			Destroy (obj);
		}
		objList.Clear ();
		TrackPos = new Vector3 (0, 0, 0);
		TrackDirection = 0;
		for (int i = 0; i < SectionList.Count; i += 1) {
			for (int x = 0; x < SectionList [i].GetComponent<SectionGen> ().PieceList.Count; ++x) {
				//Creates Rotation for newly spawned map pieces
				//Creates the new Map Piece at TrackPos Position with rot Rotation
				GameObject obj = (GameObject)GameObject.Instantiate (SectionList [i].GetComponent<SectionGen> ().PieceList[x], TrackPos, Quaternion.Euler (0, TrackDirection, 0));
				obj.GetComponent<Transform> ().rotation.Set (0, TrackDirection, 0, 0);
				obj.transform.SetParent (gameObject.transform);
				objList.Add (obj);
				//Checks if the current piece is a corner, if so:
				if (SectionList [i].tag == "Corner") {
					//If the piece is flagged as a Left Corner:
					if (SectionList [i].GetComponent<CornerCheck> ().LeftCorner) {
						//If the Current Direction Isn't North, reduce it by 90, if it is north, set it to west (because West is 270, and wont wirh with -90)
					
						TrackDirection -= 90;
						if (TrackDirection == -90) {
							TrackDirection = 270;
						}
					} else {
						//If the Current Direction isn't West, Add 90 to it, if it is, set it to North (as North = 0, not 360)
					
						TrackDirection += 90;
						if (TrackDirection == 360) {
							TrackDirection = 0;
						}
					}
				}
			
				//Adjusts the Position of the next block position based on what direction we're heading in
				switch (TrackDirection) {
				case 0:
					TrackPos.z += PieceLength;
					break;
				case 90:
					TrackPos.x += PieceLength;
					break;
				case 180:
					TrackPos.z -= PieceLength;
					break;
				case 270:
					TrackPos.x -= PieceLength;
					break;
				default:
					break;
				}
			
			}
		}
		TrackDirection = 0;
	}
}
