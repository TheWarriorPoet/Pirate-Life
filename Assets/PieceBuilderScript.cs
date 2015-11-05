using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PieceBuilderScript : MonoBehaviour {

	public MapPiece mapPiece;
	public Dropdown LeftLaneDropDown, MidLaneDropDown, RightLaneDropDown;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnChange()
	{
		mapPiece.LeftLane = LeftLaneDropDown.GetComponent<MapPieceList> ().PieceList [LeftLaneDropDown.value];
		mapPiece.MiddleLane = MidLaneDropDown.GetComponent<MapPieceList> ().PieceList [MidLaneDropDown.value];
		mapPiece.RightLane = RightLaneDropDown.GetComponent<MapPieceList> ().PieceList [RightLaneDropDown.value];
	}
}
