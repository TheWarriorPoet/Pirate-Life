using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UpgradeManager : MonoBehaviour {
    //public List<UpgradeStruct> ActiveUpgrades = new List<UpgradeStruct>();
    public List<UpgradeStruct> ActiveBoosts = new List<UpgradeStruct>();
    public Renderer PlayerRenderer = null;
    public Renderer ParrotRenderer = null;

    private UpgradeStruct _activeUpgrade = null;
    private GameManager _gameManager = null;
	// Use this for initialization
	void Start () {
        _gameManager = GameManager.instance;
        if (_gameManager != null)
        {
            foreach (UpgradeStruct us in _gameManager._allUpgrades)
            {
                if (us.name == "Pirate's Parrot" && us.Purchased)
                {
                    if (ParrotRenderer != null)
                    {
                        ParrotRenderer.enabled = true;
                    }
                }
                else if (us.Active)
                {
                    if (us.type == UpgradeBoostGold.Upgrade)
                    {
                        //ActiveUpgrades.Add(us);
                        _activeUpgrade = us;
                    }
                    else if (us.type == UpgradeBoostGold.Boost)
                    {
                        if (us.BoostsAvailable > 0)
                        {
                            ActiveBoosts.Add(us);
                        }
                        else 
                        {
                            us.Active = false;
                        }
                    }
                }
            }
        }
        if (PlayerRenderer != null)
        {
            PlayerRenderer.material = _activeUpgrade.upgradeMaterial;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
