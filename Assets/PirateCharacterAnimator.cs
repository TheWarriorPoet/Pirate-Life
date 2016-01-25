using UnityEngine;
using System.Collections;

public class PirateCharacterAnimator : MonoBehaviour {

    public Player PiratePlayer;
    public Animator PlayerAnimator;

    float NewPlayerSpeedMult;
    CharacterJoint[] MyCharJoints;

    float LastDrunkenValue;
    // Use this for initialization
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();

        PiratePlayer = GetComponentInParent<Player>();
        PlayerAnimator.enabled = true;
        ResetRagdoll();
    }


	// Update is called once per frame
	void Update ()
    {        
        NewPlayerSpeedMult = Mathf.Clamp(((float)PiratePlayer.drunkenness / 100f), 0, 1);
        PlayerAnimator.SetFloat("PlayerSpeed", NewPlayerSpeedMult);

        if (Input.GetButtonDown("TestRagdoll"))
        {
            GoToRagdoll();
        }

        if (Input.GetButtonDown("ResetRagdoll"))
        {
            ResetRagdoll();
        }
    }

    public void GoToRagdoll()
    {

        Rigidbody[] CharRbs = gameObject.GetComponentsInChildren<Rigidbody>();
        PlayerAnimator.enabled = false;

        foreach (Rigidbody RB in CharRbs)
        {
            RB.isKinematic = false;
            RB.detectCollisions = true;
            Debug.Log(RB.name + "Got listed");
        }

    }
    //Use this function to reset the player from ragdolling and dying.
    public void ResetRagdoll()
    {
        Rigidbody[] CharRbs = gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody RB in CharRbs)
        {
            PlayerAnimator.enabled = true;
            RB.isKinematic = true;
            RB.detectCollisions = false;
        }
    }
}


