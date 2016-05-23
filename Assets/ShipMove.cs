using UnityEngine;
using System.Collections;

public class ShipMove : MonoBehaviour {
	public float Speed;
	public GameObject Player = null;
	Rigidbody rb;
	public bool isReversed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Player != null)
        {
            Vector3 playerPos = Player.transform.position;
            playerPos.y = transform.position.y;
            if (isReversed)
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.right, transform.position - playerPos);
            }
            else
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.right, playerPos - transform.position);
            }
        }
        else { Debug.Log("Player is null"); }
		transform.position += (transform.forward * (Speed * Time.deltaTime));
	}
}
