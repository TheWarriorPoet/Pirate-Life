using UnityEngine;
using System.Collections;

public enum PickupType
{
	COIN,
	RUM,
	COFFEE,
}

public class PickupScript : MonoBehaviour
{
	public PickupType pickupType;
	public AudioClip soundEffect;
	public int itemValue;
	public GameObject particleSystem;
	private Player player;
	private Renderer render;
	private Transform playerTransform = null;
    private Transform objectTransform = null;
    private SceneManager_Andrew _mySceneManager = null;
	
	// Use this for initialization
	void Start()
	{
        objectTransform = transform;
        GameObject temp = GameObject.Find("Player");
        player = temp.GetComponent<Player>();
        playerTransform = temp.transform;
        _mySceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager_Andrew>();
		render = GetComponent<Renderer>();

		// Rotate randomly
		transform.Rotate(new Vector3(0, Random.Range(0.0f, 360.0f), 0));
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
			if (soundEffect != null)
			{
				AudioSource.PlayClipAtPoint(soundEffect, transform.position);
			}

			Debug.Log("Picked up " + name);

			// Insert Reaction Code here
			switch (pickupType)
			{
			case PickupType.COIN:
				if (_mySceneManager != null) {
					_mySceneManager.AddCoins (itemValue);
				}
				GameObject obj = (GameObject)GameObject.Instantiate (particleSystem, transform.position, Quaternion.identity);
				obj.transform.parent = Other.transform;	
				break;
				case PickupType.RUM:
					player.GetDrunk(itemValue);
					break;
				case PickupType.COFFEE:
					player.SoberUp(itemValue);
					break;
			}
			Destroy(gameObject);
		}
	}

	void SetAlpha(float value)
	{
		Color c = render.material.color;
		c.a = value;
		render.material.color = c;
	}
}
