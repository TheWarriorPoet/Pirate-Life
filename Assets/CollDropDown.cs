using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollDropDown : MonoBehaviour {
	public InputField iF;
	private Dropdown dd;
	// Use this for initialization
	void Start () {
		dd = GetComponent<Dropdown> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateDropDownbox()
	{
		dd.options.Clear ();
		dd.options.Capacity = int.Parse (iF.text);

		for (int i = 0; i < dd.options.Capacity; ++i) {
			dd.options.Add(new Dropdown.OptionData((i+1).ToString()));
		}
	}
}
