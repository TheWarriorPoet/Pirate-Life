using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour
{
	Vector3 velocity;

	Player player;

	void Start()
	{
		player = FindObjectOfType<Player>();
		velocity = transform.forward * player.runSpeed;
		velocity.y = 4;
	}

	void Update()
	{
		Vector3 pos = transform.position;

		//velocity.y -= 10 * Time.deltaTime;

		pos += velocity * Time.deltaTime;

		transform.position = pos;

		// Cleanup
		if (pos.y <= -4)
		{
			Destroy(gameObject);
		}
	}
}
