using UnityEngine;
using System.Collections;

public class CannonShooter : MonoBehaviour
{
	public GameObject ballPrefab, effectPrefab;
	public float activateRadius;
	public bool hasFired;

	AudioSource audioSrc;
	Player player;

	void Start()
	{
		audioSrc = GetComponent<AudioSource>();
		player = FindObjectOfType<Player>();
	}

	void Update()
	{
		float distance = Vector3.Distance(transform.position, player.transform.position);

		if (!hasFired && distance < activateRadius)
		{
			Instantiate(ballPrefab, transform.position, transform.rotation);
			Instantiate(effectPrefab, transform.position, transform.rotation);
			audioSrc.Play();
			hasFired = true;
			Debug.Log("BOMBS AWAY!");
		}
	}
}
