using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrunkEffect : MonoBehaviour {

	Image img;
	Player player;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
		player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		Color c = img.color;
		c.a = (player.drunkenness / 100.0f) / 5.0f;
		img.color = c;
	}
}
