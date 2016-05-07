using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {
	public float Lifetime;
	float timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > Lifetime) {
			GameObject.Destroy(gameObject);
		}

	}
}
