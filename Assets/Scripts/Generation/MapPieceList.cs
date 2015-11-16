using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapPieceList : MonoBehaviour {
	public List<GameObject> PieceList;
	private Dropdown dp;
	private PieceBuilderScript pbs;

	// Use this for initialization
	void Start () {
		pbs = gameObject.GetComponent<PieceBuilderScript> ();
		pbs.LeftLaneDropDown.options.Clear ();
		pbs.LeftLaneDropDown.options.Capacity = PieceList.Capacity;
		for (int i = 0; i < PieceList.Capacity; ++i) {
			pbs.LeftLaneDropDown.options.Add (new Dropdown.OptionData(PieceList[i].name));
		}

		pbs.RightLaneDropDown.options.Clear ();
		pbs.RightLaneDropDown.options.Capacity = PieceList.Capacity;
		for (int i = 0; i < PieceList.Capacity; ++i) {
			pbs.RightLaneDropDown.options.Add (new Dropdown.OptionData(PieceList[i].name));
		}

		pbs.MidLaneDropDown.options.Clear ();
		pbs.MidLaneDropDown.options.Capacity = PieceList.Capacity;
		for (int i = 0; i < PieceList.Capacity; ++i) {
			pbs.MidLaneDropDown.options.Add (new Dropdown.OptionData(PieceList[i].name));
		}

		pbs.lE.LanePieceDropDown.options.Clear ();
		pbs.lE.LanePieceDropDown.options.Capacity = PieceList.Capacity;
		for (int i = 0; i < PieceList.Capacity; ++i) {
			pbs.lE.LanePieceDropDown.options.Add (new Dropdown.OptionData(PieceList[i].name));
		}

	}
	
	// Update is called once per frame
	void Update () {

	}
}
