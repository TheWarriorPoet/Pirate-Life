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
	private Renderer renderer;
	private Transform playerTransform = null;
    private Transform objectTransform = null;
    private SceneManager_Andrew _mySceneManager = null;
	private AudioClip soundEffect;
	public int itemValue;
	// Use this for initialization
	void Start()
	{
        objectTransform = transform;
        GameObject temp = GameObject.Find("Player");
        player = temp.GetComponent<Player>();
        playerTransform = temp.transform;
        _mySceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager_Andrew>();
		renderer = GetComponent<Renderer>();

		// Set sound effect
		switch (pickupType)
		{
			case PickupType.COIN:
				soundEffect = (AudioClip) Resources.Load("Sounds/player_coin");
				break;
			case PickupType.RUM:
				soundEffect = (AudioClip)Resources.Load("Sounds/player_drink");
				break;
			case PickupType.WATER:
				soundEffect = (AudioClip)Resources.Load("Sounds/player_drink");
				break;
		}
	}

    // Update is called once per frame
    void Update()
    {
		Vector3 directionToTarget = transform.position - player.transform.position;
		float angle = Vector3.Angle(player.transform.forward, directionToTarget);

		if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
		{
			SetAlpha(0.5f);
			//Debug.Log("target is in front of me");
		}
		if (Mathf.Abs(angle) < 90 || Mathf.Abs(angle) > 270)
		{
			SetAlpha(1.0f);
			//Debug.Log("target is behind me");
		}
	}

	void OnTriggerEnter(Collider Other)
	{
		if (Other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			AudioSource.PlayClipAtPoint(soundEffect, transform.position);

			// Insert Reaction Code here
			switch (pickupType)
			{
				case PickupType.COIN:
					if (_mySceneManager != null)
                    {
                        _mySceneManager.AddCoins(itemValue);
                    }
					break;
				case PickupType.RUM:
					Debug.Log("Picked up rum");
					player.GetDrunk(itemValue);
					break;
				case PickupType.WATER:
					Debug.Log("Picked up water");
					player.SoberUp(itemValue);
					break;
			}
			Destroy(gameObject);
		}
	}

	void SetAlpha(float value)
	{
		if (value < 1.0f)
		{
			renderer.material.shader = Shader.Find("Transparent/Diffuse");
			Color c = renderer.material.color;
			c.a = value;
			renderer.material.color = c;
		}
		else
		{
			renderer.material.shader = Shader.Find("Mobile/Diffuse");
		}
	}
}
