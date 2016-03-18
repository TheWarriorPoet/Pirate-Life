using UnityEngine;
using System.Collections;

public class PlayerFootstepsAudio : MonoBehaviour
{
	public AudioClip[] footsteps;
	AudioSource audioSource;
	Player player;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		player = GetComponentInParent<Player>();
	}

	void Update()
	{
	
	}

	void PlayFootsteps()
	{
		audioSource.clip = footsteps[Random.Range(0, footsteps.Length)];
		audioSource.Play();
	}

	void FootstepsWalk()
	{
		if (player.drunkenness >= 50 || player.jumping)
		{
			return;
		}

		audioSource.volume = 0.2f;
		PlayFootsteps();
	}

	void FootstepsRun()
	{
		if (player.drunkenness < 50 || player.jumping)
		{
			return;
		}

		audioSource.volume = 0.4f;
		PlayFootsteps();
	}
}
