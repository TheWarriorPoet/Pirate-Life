using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SavePrefabScript : MonoBehaviour 
{
	public GameObject MapPiece;
	public InputField IF;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SavePrefab()
	{
		MapPiece.name = IF.text;
		UnityEditor.PrefabUtility.CreatePrefab ("Assets/Prefabs/LevelObjects/" + MapPiece.name + ".prefab", MapPiece);
		MapPiece.name = "MapPiece";
	}
}