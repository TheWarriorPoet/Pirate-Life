using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Purchase : MonoBehaviour {
    private GameManager _myGameManager = null;
    private UpgradeStruct _upgrade = null;
    public Text Title = null;
    public Text TitleShadow = null;
    public Text Cost = null;
    public Text CostShadow = null;
    public Image CoinImage = null;
    private SceneManager_Store _storeSceneManager = null;
	// Use this for initialization
	void Start () {
        _myGameManager = GameManager.instance;
        _storeSceneManager = SceneManager_Store.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetupButton(UpgradeStruct us)
    {
        gameObject.name = us.name;
        _upgrade = us;
        if (Title != null && TitleShadow != null)
        {
            Title.text = us.name;
            TitleShadow.text = us.name;
        }
        if (Cost != null && CostShadow != null)
        {
            switch (us.type) {
                case UpgradeBoostGold.Gold:
                    Cost.text = "$" + (us.MoneyCost / 100.0f).ToString();
                    CostShadow.text = "$" + (us.MoneyCost / 100.0f).ToString();
                    CoinImage.enabled = false;
                    break;
                case UpgradeBoostGold.Boost:
                case UpgradeBoostGold.Upgrade:
                    if (us.Active)
                    {
                        Cost.text = "Active";
                        CostShadow.text = "Active";
                    }
                    else if (us.Purchased)
                    {
                        Cost.text = "Bought";
                        CostShadow.text = "Bought";
                    }
                    else
                    {
                        Cost.text = us.CoinCost.ToString();
                        CostShadow.text = us.CoinCost.ToString();
                    }
                    break;
            }
        }
    }

    public void PurchaseUpgrade()
    {
        if (_myGameManager != null && _upgrade != null)
        {
            switch (_upgrade.type)
            {
                case UpgradeBoostGold.Gold:
                    _myGameManager.AddCoins(_upgrade.CoinCost);
                    // Add Google Store integration here
                    break;
                case UpgradeBoostGold.Boost:
                    if (_myGameManager.m_CoinScore >= _upgrade.CoinCost)
                    {
                        foreach (UpgradeStruct us in _myGameManager._allUpgrades)
                        {
                            if (us.name == _upgrade.name)
                            {
                                _myGameManager.AddCoins(-us.CoinCost);
                                us.Active = true;
                                us.BoostsAvailable += _upgrade.BoostsPerPurchase;
                                break;
                            }
                        }
                    }
                    break;
                case UpgradeBoostGold.Upgrade:
                    if (_myGameManager.m_CoinScore >= _upgrade.CoinCost)
                    {
                        foreach (UpgradeStruct us in _myGameManager._allUpgrades)
                        {
                            if (us.name == _upgrade.name && !us.Purchased)
                            {
                                _myGameManager.AddCoins(-us.CoinCost);
                                us.Purchased = true;
                                foreach (UpgradeStruct us2 in _myGameManager._allUpgrades)
                                {
                                    if (us2.type == UpgradeBoostGold.Upgrade && us2.Purchased && us2.Active)
                                    {
                                        us2.Active = false;
                                    }
                                }
                                us.Active = true;
                                _storeSceneManager.UpdateButtons();
                                break;
                            }
                            else if (us.name == _upgrade.name && us.Purchased)
                            {
                                foreach (UpgradeStruct us2 in _myGameManager._allUpgrades)
                                {
                                    if (us2.type == UpgradeBoostGold.Upgrade && us2.Purchased && us2.Active)
                                    {
                                        us2.Active = false;
                                    }
                                }
                                us.Active = true;
                                _storeSceneManager.UpdateButtons();
                            }
                        }
                    }
                    break;
            }
            _myGameManager.Save();
        }
        /*if (_myGameManager != null)
        {
            foreach (UpgradeStruct us in _myGameManager._allUpgrades)
            {
                if (us.name == gameObject.name && us.type == UpgradeBoostGold.Gold)
                {
                    _myGameManager.AddCoins(us.CoinCost);
                    return;
                }
                else if (us.name == gameObject.name && us. _myGameManager.m_CoinScore >= us.CoinCost)
                {
                    _myGameManager.AddCoins(-us.CoinCost);
                    us.Active = true;
                }
            }
        }*/
    }
}
