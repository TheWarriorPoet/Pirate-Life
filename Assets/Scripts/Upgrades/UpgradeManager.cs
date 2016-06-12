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

    private Player _player = null;

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
        _player = Player.instance;
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
                        Color c;
                        switch (us.upgradeValues[0].upgradeType)
                        {
                            case UpgradeType.BackMaterial:
                                PlayerMaterials[1] = us.upgradeMaterial;
                                c = us.upgradeValues[0].color;
                                c.a = 0.5f;
                                PlayerMaterials[1].color = c;
                                c.a = 1.0f;
                                PlayerMaterials[1].color = c;
                                break;
                            case UpgradeType.HatMaterial:
                                PlayerMaterials[2] = us.upgradeMaterial;
                                c = us.upgradeValues[0].color;
                                c.a = 0.5f;
                                PlayerMaterials[2].color = c;
                                c.a = 1.0f;
                                PlayerMaterials[2].color = c; break;
                            case UpgradeType.FeatherMaterial:
                                PlayerMaterials[3] = us.upgradeMaterial;
                                c = us.upgradeValues[0].color;
                                c.a = 0.5f;
                                PlayerMaterials[3].color = c;
                                c.a = 1.0f;
                                PlayerMaterials[3].color = c; break;
                            case UpgradeType.PantsMaterial:
                                PlayerMaterials[4] = us.upgradeMaterial;
                                c = us.upgradeValues[0].color;
                                c.a = 0.5f;
                                PlayerMaterials[4].color = c;
                                c.a = 1.0f;
                                PlayerMaterials[4].color = c; break;
                            case UpgradeType.ParrotUpgrade:
                                if (ParrotRenderer != null)
                                {
                                    ParrotRenderer.enabled = true;
                                }
                                break;
                            case UpgradeType.BodyMaterial:
                                PlayerMaterials[0] = us.upgradeMaterial;
                                c = us.upgradeValues[0].color;
                                c.a = 0.5f;
                                PlayerMaterials[0].color = c;
                                c.a = 1.0f;
                                PlayerMaterials[0].color = c; break;
                        }
                    }
                    else if (_player != null && us.type == UpgradeBoostGold.Boost)
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

    void Start()
    {
        foreach (UpgradeStruct us in ActiveBoosts)
        {
            switch (us.upgradeValues[0].upgradeType)
            {
                case UpgradeType.DrunkenStart:
                    _player.GetDrunk((int)us.upgradeValues[0].value);
                    break;
                case UpgradeType.PiratesTreasure:
                    SceneManager_Andrew sm = SceneManager_Andrew.instance;
                    if (sm != null)
                    {
                        sm.coinMultiplier = (int)us.upgradeValues[0].value;
                    }
                    break;
            }
        }
        if (PlayerRenderer.material.HasProperty("_Color"))
        {
            Vector4 alpha = PlayerRenderer.material.color;
            alpha.w = 0.5f;
            PlayerRenderer.material.color = alpha;
        }
        if (PlayerRenderer.material.HasProperty("_Color"))
        {
            Vector4 alpha = PlayerRenderer.material.color;
            alpha.w = 1.0f;
            PlayerRenderer.material.color = alpha;
        }
        else PlayerRenderer.enabled = true;
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
