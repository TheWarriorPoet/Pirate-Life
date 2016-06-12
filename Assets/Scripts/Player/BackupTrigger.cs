using UnityEngine;
using System.Collections;

public class BackupTrigger : MonoBehaviour {
    private Player _player;
    private Collider _collider;
	// Use this for initialization
	void Start () {
        _player = Player.instance;
        if (_player != null)
        {
            _collider = GetComponent<Collider>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (_player != null && _player._headStarting)
        {
            _collider.enabled = true;
        }
        else
            _collider.enabled = false;
	}

    void OnTriggerStay(Collider other)
    {
        //other.gameObject.SetActive(false);
    }
}
