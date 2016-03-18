using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PieceBuilderScript : MonoBehaviour {

	public MapPiece mapPiece;
	public Dropdown LeftLaneDropDown, MidLaneDropDown, RightLaneDropDown;
	public InputField LX, LY, LZ, MX, MY, MZ, RX, RY, RZ;
	private MapPieceList pieceList;

	public LaneEditor lE;
	// Use this for initialization
	/*
	void Start () {
		pieceList = GetComponent<MapPieceList> ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnChange()
	{
		mapPiece.LeftLane = pieceList.PieceList [LeftLaneDropDown.value];
		mapPiece.MiddleLane = pieceList.PieceList [MidLaneDropDown.value];
		mapPiece.RightLane = pieceList.PieceList [RightLaneDropDown.value];



		/*
		lE.lanePiece.MiddleLane = pieceList.PieceList [lE.LanePieceDropDown.value];
		DestroyImmediate (lE.lanePiece.MiddleLane.GetComponent<PickupLayout> (),true);
		lE.lanePiece.MiddleLane.AddComponent<PickupLayout> ();
		*/
	/*
		try
		{
		mapPiece.LeftLaneOffset = new Vector3 (float.Parse(LX.text), float.Parse(LY.text), float.Parse(LZ.text));
		mapPiece.MiddleLaneOffset = new Vector3 (float.Parse(MX.text), float.Parse(MY.text), float.Parse(MZ.text));
		mapPiece.RightLaneOffset = new Vector3 (float.Parse(RX.text), float.Parse(RY.text), float.Parse(RZ.text));
		}
		catch(UnityException e)
		{
			mapPiece.LeftLaneOffset = new Vector3 (-3, 0, 0);
			mapPiece.MiddleLaneOffset = new Vector3 (0, 0, 0);
			mapPiece.RightLaneOffset = new Vector3 (3, 0, 0);
		}
	}
	*/
}
