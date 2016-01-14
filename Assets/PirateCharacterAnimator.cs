using UnityEngine;
using System.Collections;

public class PirateCharacterAnimator : MonoBehaviour {

    public Player PiratePlayer;
    public Animator PlayerAnimator;

    float NewPlayerSpeedMult;


    float LastDrunkenValue;
    // Use this for initialization
    void Start ()
    {
        PlayerAnimator = GetComponent<Animator>();
    
        PiratePlayer = GetComponentInParent<Player>();
        PlayerAnimator.enabled = true;
    }
	
	// Update is called once per frame
	void Update ()
    {

        //NewPlayerSpeedMult = (float)PiratePlayer.drunkenness / 100f;
        NewPlayerSpeedMult = Mathf.Clamp(((float)PiratePlayer.drunkenness / 100f), 0, 1);
        PlayerAnimator.SetFloat("PlayerSpeed", NewPlayerSpeedMult);
       // Debug.Log(NewPlayerSpeedMult);

    }

    public void GoToRagdoll()
    {
        PlayerAnimator.enabled = false;
    }

}
