using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SectionEditorUI : MonoBehaviour {
	public LevelGen LG;
	public SectionGen SG;
	public Dropdown PieceListDD;
	public InputField IF;
	public MapPieceList MPL;
	public GameObject CameraHolder;
	public int SelectedIndex;
	public Text IndexText;
	private int SectionSize;

	// Use this for initialization
	void Start () {
		UpdateDropdown ();
	}

	public void UpdateDropdown()
	{
		PieceListDD.ClearOptions ();
		foreach(GameObject obj in MPL.PieceList)
		{
			PieceListDD.options.Add (new Dropdown.OptionData(obj.name));
		}
	}

	public void UpdatePiece()
	{
		SG.PieceList [SelectedIndex] = MPL.PieceList [PieceListDD.value];
	}

	public void NextPiece()
	{
		if (SelectedIndex < SG.PieceList.Count - 1) {
			SelectedIndex += 1;
			IndexText.text = (SelectedIndex + 1).ToString();
			CameraHolder.transform.position = new Vector3 (0, 0, SelectedIndex * 50);
		}
	}

	public void PrevPiece()
	{
		if (SelectedIndex > 0) {
			SelectedIndex -= 1;
			IndexText.text = (SelectedIndex + 1).ToString();
			CameraHolder.transform.position = new Vector3 (0, 0, SelectedIndex * 50);
		}
	}

	public void UpdateSectionSize()
	{
		int.TryParse (IF.text,out SectionSize);
		while (SG.PieceList.Count > SectionSize) {
			GameObject.Destroy (SG.PieceList[SG.PieceList.Count - 1]);
			SG.PieceList.RemoveAt (SG.PieceList.Count - 1);
		}
		while (SG.PieceList.Count < SectionSize) {
			SG.PieceList.Add (SG.PieceList [SG.PieceList.Count - 1]);
		}
		LG.RebuildMap ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
