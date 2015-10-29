using UnityEngine;
using System.Collections;

public enum PickupType
{
	COIN,
	RUM,
	WATER,
}
public class PickupScript : MonoBehaviour {

	public GameObject Player;
	public PickupType pickupType;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider Other)
	{
		if (Other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Insert Reaction Code here
			Debug.Log("AA");
			Destroy(transform.parent.gameObject);
		}
	}
}
