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
    private Transform playerTransform = null;
    private Transform objectTransform = null;
    private SceneManager_Andrew _mySceneManager = null;
    private bool MagneticCoin = false;
    public float magnetSpeed = 20.0f;
	// Use this for initialization
	void Start()
	{
        objectTransform = transform;
        GameObject temp = GameObject.Find("Player");
        player = temp.GetComponent<Player>();
        playerTransform = temp.transform;
        _mySceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager_Andrew>();
        if (_mySceneManager != null && pickupType == PickupType.COIN && _mySceneManager.magneticCoins)
        {
            MagneticCoin = true;
        }
	}
	
	// Update is called once per frame
	void Update()
	{
        if (MagneticCoin)
        {
            objectTransform.position = Vector3.MoveTowards(objectTransform.position, playerTransform.position, magnetSpeed * Time.deltaTime);
        }
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
			Destroy(gameObject);
		}
	}
}
