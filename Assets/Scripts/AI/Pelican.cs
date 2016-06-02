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
	Animator anim;
	Vector3 pukePos;
	float pukeTimer;

	void Start()
	{
		player = FindObjectOfType<Player>();
		anim = GetComponent<Animator>();
		vomit = transform.parent.gameObject.GetComponentInChildren<ParticleSystem>();

		anim.speed = 0;
	}

	void Update()
	{
		if (player == null) return;

		float distance = Vector3.Distance(transform.position, player.transform.position);

		if (!hasPuked && distance < activateRadius)
		{
			//SpawnPuke();
			anim.speed = 1;
			anim.Play("PUKE");
		}

		if (hasPuked && vomit.isPlaying)
		{
			pukeTimer += Time.deltaTime;
			if (pukeTimer >= pukeDuration)
			{
				vomit.Stop();
				//anim.speed = 0;
			}
		}
	}

	void SpawnPuke()
	{
		pukedObject = (GameObject)Instantiate(pukePrefab, pukePos, Quaternion.identity);
	}

	public void Puke()
	{
		Vector3 target = (transform.position + transform.up) - transform.right * pukeDistance;

		Debug.DrawLine(target, target - transform.up * 10);

		RaycastHit hit;
		if (Physics.Raycast(target, -transform.up, out hit, 10))
		{
			pukePos = hit.point;
			Invoke("SpawnPuke", 0.5f);
			Debug.Log("PUKE!");
			hasPuked = true;
			vomit.Play();
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