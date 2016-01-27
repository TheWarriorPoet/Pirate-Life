using UnityEngine;
using System.Collections;

public class PlayerFootstepsAudio : MonoBehaviour {

	public AudioClip[] footsteps;
	AudioSource audioSource;
	Player player;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		player = GetComponentInParent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlayFootsteps()
	{
		audioSource.clip = footsteps[Random.Range(0, footsteps.Length)];
		audioSource.Play();
		Debug.Log("PLAYED SOUND");
	}

	void FootstepsWalk()
	{
		if (player.drunkenness >= 50 || player.jumping)
		{
			return;
		}

		PlayFootsteps();
	}

	void FootstepsRun()
	{
		if (player.drunkenness < 50 || player.jumping)
		{
			return;
		}

		PlayFootsteps();
	}
}
