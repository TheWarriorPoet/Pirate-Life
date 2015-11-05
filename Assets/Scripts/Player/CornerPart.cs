using UnityEngine;
using System.Collections;

public class CornerPart : MonoBehaviour {
	public CornerCheck cornerCheck;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider Other)
	{
		if (Other.tag == "Player") {
			if(!cornerCheck.hasTurned)
			{
				if(Input.GetAxis ("Horizontal") > 0)
				{
					Other.transform.Rotate(new Vector3(0,90,0));
					Other.transform.position = new Vector3(transform.position.x, Other.transform.position.y, transform.position.z);
					cornerCheck.hasTurned = true;
				}else if (Input.GetAxis ("Horizontal") < 0)
				{
					Other.transform.Rotate(new Vector3(0,-90,0));
					Other.transform.position = new Vector3(transform.position.x, Other.transform.position.y, transform.position.z);
					cornerCheck.hasTurned = true;
				}
			}
		}
	}
}
