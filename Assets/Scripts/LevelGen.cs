using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {
	public float PieceZLength = 50;
	public List<GameObject> PieceList;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < PieceList.Count; i+=1) {
			GameObject.Instantiate(PieceList[i], new Vector3(0,0,i*PieceZLength), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
