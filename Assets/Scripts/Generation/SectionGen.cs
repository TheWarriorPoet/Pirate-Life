﻿using UnityEngine;
using System.Collections.Generic;

public class SectionGen : MonoBehaviour {
	public bool RandomGen;
<<<<<<< HEAD
	public int MaxSectionSize, MinSectionSize;
=======
>>>>>>> 4b15c46... Added cannons to the procgenerator, these fire cannon balls when the player moves within range,
	public List<GameObject> FullPieceList = new List<GameObject> ();
	public List<GameObject> PieceList = new List<GameObject>();
	// Use this for initialization
	void Start () {
		if (RandomGen) {
<<<<<<< HEAD
			GenerateSection ();
		}
	}

	public void GenerateSection()
	{
		if (RandomGen) {
			Random.seed = (int)System.DateTime.Now.Ticks;
			int Size = Random.Range (MinSectionSize, MaxSectionSize);
		
			PieceList.Clear ();

			PieceList.Add (FullPieceList [Random.Range (0, FullPieceList.Count - 1)]);
			for (int i = 1; i < Size; i++) {
				List<GameObject> tempList = PieceList [PieceList.Count - 1].GetComponent<MapPiece> ().NextPieces;
				PieceList.Add (tempList [Random.Range (0, tempList.Count - 1)]);
			}
=======
			GenerateSection (3);
		}
	}

	public void GenerateSection(int Size)
	{
		PieceList.Clear ();
		Random.seed = (int)System.DateTime.Now.Ticks;
		PieceList.Add (FullPieceList [Random.Range (0, FullPieceList.Count - 1)]);
		for (int i = 1; i < Size; i++) {
			List<GameObject> tempList = PieceList[PieceList.Count - 1].GetComponent<MapPiece>().NextPieces;
			PieceList.Add (tempList [Random.Range (0, tempList.Count - 1)]);
>>>>>>> 4b15c46... Added cannons to the procgenerator, these fire cannon balls when the player moves within range,
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
