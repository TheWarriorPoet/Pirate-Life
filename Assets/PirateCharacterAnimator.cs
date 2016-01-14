using UnityEngine;
using System.Collections;

public class PirateCharacterAnimator : MonoBehaviour {

    Player PiratePlayer;
    Animator PlayerAnimator;

    float NewPlayerSpeedMult;


    float LastDrunkenValue;
    // Use this for initialization
    void Start ()
    {
        PlayerAnimator = GetComponent<Animator>();
    
        PiratePlayer = GetComponentInParent<Player>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        NewPlayerSpeedMult = PiratePlayer.drunkenness / 100;

        PlayerAnimator.SetFloat("PlayerSpeed", NewPlayerSpeedMult);
    }

}
