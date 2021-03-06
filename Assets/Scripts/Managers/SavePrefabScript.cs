﻿#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SavePrefabScript : MonoBehaviour 
{
	public GameObject MapPiece;
	public InputField IF;
	public string Destination;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SavePrefab()
	{
		while(MapPiece.transform.childCount > 0)
		{
			GameObject.Destroy (MapPiece.transform.GetChild (MapPiece.transform.childCount - 1).gameObject);
			MapPiece.transform.GetChild (MapPiece.transform.childCount - 1).parent = null;
		}
		MapPiece.name = IF.text;
		UnityEditor.PrefabUtility.CreatePrefab (Destination + MapPiece.name + ".prefab", MapPiece);
		MapPiece.name = "MapPiece";

		if (MapPiece.GetComponent<LevelGen> () != null) {
			MapPiece.tag = "LevelGen";
			MapPiece.name = "LevelGenerator";
		}
		MapPiece.GetComponent<MapPiece> ().BuildPiece ();
	}
}

#endif