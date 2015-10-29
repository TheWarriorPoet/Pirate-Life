using UnityEngine;
using System.Collections;

public enum PickupType
{
	COIN,
	RUM,
	WATER,
}

public class PickupScript : MonoBehaviour
{
	public PickupType pickupType;
	private Player player;

	// Use this for initialization
	void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void OnTriggerEnter(Collider Other)
	{
		if (Other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Insert Reaction Code here
			switch (pickupType)
			{
				case PickupType.COIN:
					Debug.Log("Picked up coin");
					break;
				case PickupType.RUM:
					Debug.Log("Picked up rum");
					player.GetDrunk();
					break;
				case PickupType.WATER:
					Debug.Log("Picked up water");
					player.SoberUp();
					break;
			}
			Destroy(transform.parent.gameObject);
		}
	}
}
