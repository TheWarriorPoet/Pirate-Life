using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UpgradeManager : MonoBehaviour {
    //public List<UpgradeStruct> ActiveUpgrades = new List<UpgradeStruct>();
    public List<UpgradeStruct> ActiveBoosts = new List<UpgradeStruct>();
    public Material BlankMaterial = null;
    private Renderer PlayerRenderer = null;
    private Renderer ParrotRenderer = null;

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
        var PlayerMaterials = PlayerRenderer.materials;
        if (PlayerRenderer != null && PlayerRenderer.materials.Length == 5)
        {
            //PlayerMaterials[0] = BlankMaterial;
            PlayerMaterials[1] = BlankMaterial;
            PlayerMaterials[2] = BlankMaterial;
            PlayerMaterials[3] = BlankMaterial;
            //PlayerMaterials[4] = BlankMaterial;
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
                                PlayerMaterials[1] = us.upgradeMaterial;
                                PlayerMaterials[1].color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.HatMaterial:
                                PlayerMaterials[2] = us.upgradeMaterial;
                                PlayerMaterials[2].color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.FeatherMaterial:
                                PlayerMaterials[3] = us.upgradeMaterial;
                                PlayerMaterials[3].color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.PantsMaterial:
                                PlayerMaterials[4] = us.upgradeMaterial;
                                PlayerMaterials[4].color = us.upgradeValues[0].color;
                                break;
                            case UpgradeType.ParrotUpgrade:
                                if (ParrotRenderer != null)
                                {
                                    ParrotRenderer.enabled = true;
                                }
                                break;
                            case UpgradeType.BodyMaterial:
                                PlayerMaterials[0] = us.upgradeMaterial;
                                PlayerMaterials[0].color = us.upgradeValues[0].color;
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
            PlayerRenderer.materials = PlayerMaterials;
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

    public void DeactivateParrot()
    {
        if (ParrotRenderer != null)
        {
            ParrotRenderer.enabled = false;
        }
    }
}
