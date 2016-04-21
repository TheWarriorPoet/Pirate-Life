using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearbyCrates : MonoBehaviour {
    public List<GameObject> CratesAhead = new List<GameObject>();
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            CratesAhead.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        CratesAhead.Clear();
    }
}
