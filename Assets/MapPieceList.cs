using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapPieceList : MonoBehaviour {
	public List<GameObject> PieceList;
	private Dropdown dp;

	// Use this for initialization
	void Start () {
		dp = gameObject.GetComponent<Dropdown>();
		dp.options.Clear ();
		dp.options.Capacity = PieceList.Capacity;
		for (int i = 0; i < PieceList.Capacity; ++i) {
			dp.options.Add (new Dropdown.OptionData(PieceList[i].name));
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
