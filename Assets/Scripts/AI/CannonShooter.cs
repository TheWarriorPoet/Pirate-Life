using UnityEngine;
using System.Collections;

public class CannonShooter : MonoBehaviour
{
	public GameObject ballPrefab, effectPrefab;
	public float activateRadius;
	public bool sideCannon, hasFired;

	AudioSource audioSrc;
	Player player;
	Vector3 testStart, testDir;
	LayerMask lm;

	void Start()
	{
		lm = (1 << LayerMask.NameToLayer("Player"));
		audioSrc = GetComponent<AudioSource>();
		player = FindObjectOfType<Player>();

		testStart = transform.parent.position;
		testStart.y += 4;
		testDir = (transform.parent.right - transform.parent.forward).normalized;
	}

	void Update()
	{
		if (sideCannon)
		{
			Debug.DrawLine(testStart, testStart + testDir * 30);

			if (!hasFired && Physics.Raycast(testStart, testDir, 30.0f, lm)) //if (!hasFired && distance < activateRadius)
			{
				Shoot();
			}
		}
		else
		{
			float distance = Vector3.Distance(transform.position, player.transform.position);

			if (!hasFired && distance < activateRadius)
			{
				Shoot();
			}
		}
	}

	void Shoot()
	{
		Instantiate(ballPrefab, transform.position, transform.rotation);
		Instantiate(effectPrefab, transform.position, transform.rotation);
		audioSrc.Play();
		hasFired = true;
		Debug.Log("BOMBS AWAY!");
	}
}
