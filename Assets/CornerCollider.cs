using System.Collections;
using UnityEngine;

public class CornerCollider : MonoBehaviour {
	public GameObject Player;
	bool hasRotated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider Other)
	{
		if (Other.gameObject == Player) {
			if(!hasRotated)
			{
				if(Input.GetAxis("Horizontal") < 0)
				{
					Player.transform.Rotate(new Vector3(0,90,0));
					hasRotated = true;
				}else if(Input.GetAxis ("Horizontal") > 0)
				{
					Player.transform.Rotate(new Vector3(0,90,0));
					hasRotated = true;
				}
			}
		}
	}
	void OnTriggerExit(Collider Other)
	{
		if (Other.gameObject == Player) {
			hasRotated = false;
		}
	}
}
