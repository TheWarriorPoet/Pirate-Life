using UnityEngine;
using System.Collections;

public class SoundDelayer : MonoBehaviour
{
	public float timeDelay;
	public bool random;

	AudioSource sound;
	float delay, delayTimer;

	void Start()
	{
		sound = GetComponent<AudioSource>();
		delay = random ? RandomDelay() : timeDelay;
	}

	void Update()
	{
		if (sound.isPlaying) return;

		delayTimer += Time.deltaTime;

		if (delayTimer >= delay)
		{
			sound.Play();
			delayTimer = 0;
		}
	}

	float RandomDelay()
	{
		return Random.Range(0.0f, timeDelay);
	}
}
