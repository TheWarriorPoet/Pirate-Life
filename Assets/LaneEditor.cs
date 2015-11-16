using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LaneEditor : MonoBehaviour {
	public Dropdown LanePieceDropDown, CollectableIndexDropDown, CollectableDropdown;
	public InputField CollectableNumInput, InputCollX, InputCollY, InputCollZ;
	public MapPiece lanePiece;
	public List<GameObject> CollectableTypes;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateLane()
	{

		PickupLayout pL = lanePiece.GetComponent<PickupLayout> ();
		if(pL != null)
		{
			pL.Collectables.Clear ();
			pL.Collectables.Capacity = int.Parse (CollectableNumInput.text);
			
			pL.Collectables [CollectableIndexDropDown.value] = CollectableTypes [CollectableDropdown.value];
			pL.CollectablesOffset [CollectableIndexDropDown.value] = new Vector3 (float.Parse (InputCollX.text), 
			                                                                      float.Parse (InputCollY.text),
			                                                                      float.Parse (InputCollZ.text));
		}
	}
}
