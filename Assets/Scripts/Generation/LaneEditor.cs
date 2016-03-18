using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LaneEditor : MonoBehaviour {
	public MapPiece MP;
	public Dropdown LLaneType, LCollType, LObstType, MLaneType, MCollType, MObstType, RLaneType, RCollType, RObstType;
	private CollTypeList CTL;
	// Use this for initialization
	void Start () {
		Init ();
	}

	public void Init()
	{
		CTL = GetComponent<CollTypeList> ();
		LLaneType.ClearOptions ();
		foreach (GameObject obj in CTL.LaneTypes) {
			LLaneType.options.Add (new Dropdown.OptionData (obj.name));
		}

		LCollType.ClearOptions ();
		foreach (GameObject obj in CTL.CollectableTypes) {
			LCollType.options.Add (new Dropdown.OptionData (obj.name));
		}

		LObstType.ClearOptions ();
		foreach (GameObject obj in CTL.ObstacleTypes) {
			LObstType.options.Add (new Dropdown.OptionData (obj.name));
		}

		MLaneType.ClearOptions ();
		foreach (GameObject obj in CTL.LaneTypes) {
			MLaneType.options.Add (new Dropdown.OptionData (obj.name));
		}

		MCollType.ClearOptions ();
		foreach (GameObject obj in CTL.CollectableTypes) {
			MCollType.options.Add (new Dropdown.OptionData (obj.name));
		}

		MObstType.ClearOptions ();
		foreach (GameObject obj in CTL.ObstacleTypes) {
			MObstType.options.Add (new Dropdown.OptionData (obj.name));
		}

		RLaneType.ClearOptions ();
		foreach (GameObject obj in CTL.LaneTypes) {
			RLaneType.options.Add (new Dropdown.OptionData (obj.name));
		}

		RCollType.ClearOptions ();
		foreach (GameObject obj in CTL.CollectableTypes) {
			RCollType.options.Add (new Dropdown.OptionData (obj.name));
		}

		RObstType.ClearOptions ();
		foreach (GameObject obj in CTL.ObstacleTypes) {
			RObstType.options.Add (new Dropdown.OptionData (obj.name));
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void UpdatePiece()
	{
		MP.Lanes.Clear ();
		MP.Collectables.Clear ();
		MP.Obstacles.Clear ();

		MP.Lanes.Add (CTL.LaneTypes [LLaneType.value]);
		MP.Lanes.Add (CTL.LaneTypes [MLaneType.value]);
		MP.Lanes.Add (CTL.LaneTypes [RLaneType.value]);

		MP.Collectables.Add (CTL.CollectableTypes [LCollType.value]);
		MP.Collectables.Add (CTL.CollectableTypes [MCollType.value]);
		MP.Collectables.Add (CTL.CollectableTypes [RCollType.value]);

		MP.Obstacles.Add (CTL.ObstacleTypes [LObstType.value]);
		MP.Obstacles.Add (CTL.ObstacleTypes [MObstType.value]);
		MP.Obstacles.Add (CTL.ObstacleTypes [RObstType.value]);

		MP.BuildPiece ();
	}


}
