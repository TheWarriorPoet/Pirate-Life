using UnityEngine;
using System.Collections.Generic;

public enum TrackDir
{
	NORTH = 0,
	EAST = 90,
	SOUTH = 180,
	WEST = 270,
}

public class LevelGen : MonoBehaviour {
	public TrackDir TrackDirection;
	public float PieceLength = 50;
	public GameObject CornerBlock; //This is to check if the block is a corner, and to start layering the track around the corner
	public List<GameObject> PieceList;
	private Vector3 TrackPos = new Vector3(0,0,0);
	// Use this for initialization
	void Start () {

		for (int i = 0; i < PieceList.Count; i+=1) {
			//Creates Rotation for newly spawned map pieces
			Quaternion rot = new Quaternion(0,(float)TrackDirection,0,0);
			//Creates the new Map Piece at TrackPos Position with rot Rotation
			GameObject.Instantiate(PieceList[i],TrackPos, rot);

			//Checks if the current piece is a corner, if so:
			if(PieceList[i].tag == "Corner")
			{
				//If the piece is flagged as a Left Corner:
				if(PieceList[i].GetComponent<CornerCheck>().LeftCorner)
				{
					//If the Current Direction Isn't North, reduce it by 90, if it is north, set it to west (because West is 270, and wont wirh with -90)
					if(TrackDirection != TrackDir.NORTH)
					{
						TrackDirection -= 90;
					}else 
					{
						TrackDirection = TrackDir.WEST;
					}
				}else
				{
					//If the Current Direction isn't West, Add 90 to it, if it is, set it to North (as North = 0, not 360)
					if(TrackDirection != TrackDir.WEST)
					{
						TrackDirection += 90;
					}else 
					{
						TrackDirection = TrackDir.NORTH;
					}
				}
			}

			//Adjusts the Position of the next block position based on what direction we're heading in
			switch(TrackDirection)
			{
			case TrackDir.NORTH:
				TrackPos.z += PieceLength;
				break;
			case TrackDir.EAST:
				TrackPos.x += PieceLength;
				break;
			case TrackDir.SOUTH:
				TrackPos.z -= PieceLength;
				break;
			case TrackDir.WEST:
				TrackPos.x -= PieceLength;
				break;
			default:
				break;
			}

		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
