using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagneticUpgrade : Upgrade {
    private float _magnetRadius = 100;
    private float _magnetSpeed = 10;
    private List<PickupScript> _AllCoinPickups = new List<PickupScript>();
    private GameManager _GameManager = null;
    private SphereCollider _coinRangeCollider = null;
    void Start()
    {
        Debug.Log("MagneticUpgrade Starting");
        _GameManager = GameManager.instance;
        _ScenePlayer = Player.instance;
        _coinRangeCollider = GetComponent<SphereCollider>(); ;
        if (_GameManager != null)
        {
            foreach (UpgradeStruct u in _GameManager._allUpgrades)
            {
                if (u.upgradeGOName == this.name)
                {
                    _UpgradeInfo = u;
                    u.upgradeScript = this;
                    Debug.Log("Upgrade Info Found");
                }
            }
        }
        foreach (UpgradeValue uv in _UpgradeInfo.upgradeValues)
        {
            if (uv.upgradeType == UpgradeType.CoinAttractRange)
            {
                _magnetRadius = uv.value;
                _coinRangeCollider.radius = uv.value;
            }
            else if (uv.upgradeType == UpgradeType.CoinAttractSpeed)
            {
                _magnetSpeed = uv.value;
            }
        }
    }


    public override void UpgradeUpdate()
    {
        if (!_UpgradeInfo.Active || _ScenePlayer == null) return;
        if (_ScenePlayer == null)
        {
            _ScenePlayer = Player.instance;
        }
        /*if (_GameManager != null && (_GameManager.newTrackSection || _GameManager.PlayerReset || _AllCoinPickups.Count == 0))
        {
            _AllCoinPickups.Clear();
            GameObject[] allCoins = GameObject.FindGameObjectsWithTag("Coin") as GameObject[];
            foreach (GameObject go in allCoins)
            {
                PickupScript pu = go.GetComponent<PickupScript>();
                if (pu != null && pu.pickupType == PickupType.COIN)
                {
                    _AllCoinPickups.Add(pu);
                }
            }
            _GameManager.newTrackSection = false;
            _GameManager.PlayerReset = false;
        }*/
        Debug.Log("There are " + _AllCoinPickups.Count + " coins");

        foreach (PickupScript p in _AllCoinPickups)
        {
            if (p == null) { continue; }
            /*if (Vector3.Distance(_ScenePlayer.transform.position, p.transform.position) <= _magnetRadius)
            {
                p.transform.position = Vector3.MoveTowards(p.transform.position, _ScenePlayer.transform.position, _magnetSpeed * Time.deltaTime);
            }*/
            p.transform.position = Vector3.MoveTowards(p.transform.position, _ScenePlayer.transform.position, _magnetSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Coin")
        {
            _AllCoinPickups.Add(other.GetComponent<PickupScript>());
            Debug.Log("Coin Added");
        }
    }
}
