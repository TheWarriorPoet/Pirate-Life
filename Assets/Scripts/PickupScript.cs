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
    private SceneManager_Base _mySceneManager = null;

	// Use this for initialization
	void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
        _mySceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager_Andrew>();
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
					if (_mySceneManager != null)
                    {
                        _mySceneManager.AddCoins(1);
                    }
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
