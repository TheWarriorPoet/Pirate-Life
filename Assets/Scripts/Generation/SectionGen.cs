using UnityEngine;
using System.Collections.Generic;

public class SectionGen : MonoBehaviour {
	public bool RandomGen;
	public int MaxSectionSize, MinSectionSize;
	public List<GameObject> FullPieceList = new List<GameObject> ();
	public List<GameObject> PieceList = new List<GameObject>();
	// Use this for initialization
	void Start () {
		if (RandomGen) {
			GenerateSection ();
		}
	}

	public void GenerateSection()
	{
		if (RandomGen) {
			Random.seed = (int)System.DateTime.Now.Ticks;
			int Size = Random.Range (MinSectionSize, MaxSectionSize);
		
			PieceList.Clear ();

			PieceList.Add (FullPieceList [(Random.Range (1,FullPieceList.Count)) - 1]);
			for (int i = 1; i < Size; i++) {
				List<GameObject> tempList = PieceList [PieceList.Count - 1].GetComponent<MapPiece> ().NextPieces;
				PieceList.Add (tempList [Random.Range (0, tempList.Count - 1)]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
