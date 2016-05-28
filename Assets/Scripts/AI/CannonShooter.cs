using UnityEngine;
using System.Collections;

public class CannonShooter : MonoBehaviour
{
	public GameObject ballPrefab, effectPrefab;
	public AudioClip blastSound;
	public float activateRadius;
	public bool hasFired;

	Player player;

	void Start()
	{
		player = FindObjectOfType<Player>();
	}

	void Update()
	{
		float distance = Vector3.Distance(transform.position, player.transform.position);

		if (!hasFired && distance < activateRadius)
		{
			Instantiate(ballPrefab, transform.position, transform.rotation);
			Instantiate(effectPrefab, transform.position, transform.rotation);
			AudioSource.PlayClipAtPoint(blastSound, transform.position);
			hasFired = true;
			Debug.Log("BOMBS AWAY!");
		}
	}
}
