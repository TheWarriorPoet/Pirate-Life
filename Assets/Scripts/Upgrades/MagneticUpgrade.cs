using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagneticUpgrade : Upgrade {
    protected struct PickupGameObject
    {
        public PickupScript _PickupScript;
        public Transform _PickupTransform;

        public PickupGameObject(PickupScript pu, Transform t)
        {
            _PickupScript = pu;
            _PickupTransform = t;
        }
    }
    public float magnetRadius = 100;
    public float magnetSpeed = 10;
    private List<PickupGameObject> _AllCoinPickups = new List<PickupGameObject>();
    private Transform _PlayerTransform = null;
    private GameManager _GameManager = null;
    void Start()
    {
        Debug.Log("MagneticUpgrade Starting");
        _GameManager = GameManager.instance;
        _ScenePlayer = Player.instance;
        if (_ScenePlayer != null)
        {
            _PlayerTransform = _ScenePlayer.transform;
        }
    }


    public override void UpgradeUpdate()
    {
        Debug.Log("MagneticUpgrade Updating");
        if (!UpgradeActive || _ScenePlayer == null) return;
        if (_PlayerTransform == null)
        {
            _ScenePlayer = Player.instance;
            _PlayerTransform = _ScenePlayer.transform;
            Debug.Log("Resetting Player Transform");
        }
        if (_GameManager != null && (_GameManager.newTrackSection || _AllCoinPickups.Count == 0))
        {
            _AllCoinPickups.Clear();
            GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin") as GameObject[];
            foreach (GameObject go in allCoins)
            {
                PickupScript pu = go.GetComponent<PickupScript>();
                if (pu != null && pu.pickupType == PickupType.COIN)
                {
                    _AllCoinPickups.Add(new PickupGameObject(pu, go.transform));
                }
            }
            _GameManager.newTrackSection = false;
        }
        Debug.Log("There are " + _AllCoinPickups.Count + " coins");

        foreach (PickupGameObject p in _AllCoinPickups)
        {
            if (p._PickupTransform == null) { _AllCoinPickups.Remove(p); return; }
            if (_PlayerTransform != null && Vector3.Distance(_PlayerTransform.position, p._PickupTransform.position) <= magnetRadius)
            {
                p._PickupTransform.position = Vector3.MoveTowards(p._PickupTransform.position, _PlayerTransform.position, magnetSpeed * Time.deltaTime);
            }
        }

    }
}
