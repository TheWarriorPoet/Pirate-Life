using UnityEngine;
using System.Collections;

public class Pelican : MonoBehaviour
{
	public GameObject pukePrefab;
	public float activateRadius, pukeDistance, pukeDuration;
	public bool hasPuked;

	Player player;
	GameObject pukedObject;
	ParticleSystem vomit;
	float pukeTimer;

	void Start()
	{
		player = FindObjectOfType<Player>();
		vomit = GetComponentInChildren<ParticleSystem>();
		vomit.gameObject.SetActive(false);
	}

	void Update()
	{
		if (player == null) return;

		float distance = Vector3.Distance(transform.position, player.transform.position);

		if (!hasPuked && distance < activateRadius)
		{
			SpawnPuke();
		}

		if (hasPuked && vomit.gameObject.activeInHierarchy)
		{
			pukeTimer += Time.deltaTime;
			if (pukeTimer >= pukeDuration)
			{
				vomit.gameObject.SetActive(false);
			}
		}
	}

	void SpawnPuke()
	{
		Vector3 target = (transform.position + transform.up) - transform.right * pukeDistance;

		Debug.DrawLine(target, target - transform.up * 10);

		RaycastHit hit;
		if (Physics.Raycast(target, -transform.up, out hit, 10))
		{
			pukedObject = (GameObject)Instantiate(pukePrefab, hit.point, Quaternion.identity);
			Debug.Log("PUKE!");
			hasPuked = true;
			vomit.gameObject.SetActive(true);
		}
	}

	void OnDisable()
	{
		if (pukedObject != null)
		{
			Destroy(pukedObject);
		}
	}
}