using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UpgradeManager : MonoBehaviour {
    //public List<UpgradeStruct> ActiveUpgrades = new List<UpgradeStruct>();
    public List<UpgradeStruct> ActiveBoosts = new List<UpgradeStruct>();
    public Renderer PlayerRenderer = null;
    public Renderer ParrotRenderer = null;

    private Material BaseMaterial = null;
    private Material BackDecoration = null;
    private Material HatMaterial = null;
    private Material FeatherMaterial = null;
    private Material PantsMaterial = null;

    //private UpgradeStruct _activeUpgrade = null;
    private GameManager _gameManager = null;

    // Singleton Instance to provide simple access through other scripts
    private static UpgradeManager _instance = null;
    public static UpgradeManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (UpgradeManager)FindObjectOfType(typeof(UpgradeManager));
            }
            return _instance;
        }
    }
    // Use this for initialization
    void Awake () {
        _gameManager = GameManager.instance;
        PlayerRenderer = GameObject.FindGameObjectWithTag("PirateMesh").GetComponent<Renderer>();
        ParrotRenderer = GameObject.FindGameObjectWithTag("Parrot").GetComponent<Renderer>();
        if (PlayerRenderer != null)
        {
            BaseMaterial = PlayerRenderer.materials[0];
            BackDecoration = PlayerRenderer.materials[1];
            HatMaterial = PlayerRenderer.materials[2];
            FeatherMaterial = PlayerRenderer.materials[3];
            PantsMaterial = PlayerRenderer.materials[4];
        }
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
                        //_activeUpgrade = us;
                        switch (us.upgradeValues[0].upgradeType)
                        {
                            case UpgradeType.BackMaterial:
                                BackDecoration = us.upgradeMaterial;
                                BackDecoration.color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.HatMaterial:
                                HatMaterial = us.upgradeMaterial;
                                HatMaterial.color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.FeatherMaterial:
                                FeatherMaterial = us.upgradeMaterial;
                                HatMaterial.color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.PantsMaterial:
                                PantsMaterial = us.upgradeMaterial;
                                PantsMaterial.color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.ParrotUpgrade:
                                if (ParrotRenderer != null)
                                {
                                    ParrotRenderer.enabled = true;
                                }
                                break;
                            case UpgradeType.BodyMaterial:
                                BaseMaterial = us.upgradeMaterial;
                                BaseMaterial.color = us.upgradeValues[0].color;
                                break;
                        }
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
        /*if (PlayerRenderer != null && _activeUpgrade != null)
        {
            Debug.Log("Upgrade: " + _activeUpgrade.name);
            PlayerRenderer.material = _activeUpgrade.upgradeMaterial;
            
        }*/
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DecreaseActiveBoosts()
    {
        foreach (UpgradeStruct us in ActiveBoosts)
        {
            if (us.BoostsAvailable > 0)
                us.BoostsAvailable--;
        }
    }
}
