using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Purchase : MonoBehaviour {
    private GameManager _myGameManager = null;
    private UpgradeStruct _upgrade = null;

    // Purchase Title
    public Text Title = null;
    public Text TitleShadow = null;

    // Purchase Cost
    public Text Cost = null;
    public Text CostShadow = null;
    public Image CoinImage = null;

    // You have
    public Text YouHave = null;

    // Description
    public Text Description = null;

    // Purchase Image
    public Image PurchaseSprite = null;

    // Buy/Activate Buttons
    public GameObject BuyButton = null;
    public GameObject ActiveButton = null;
    public Image ActiveImage = null;

    // Active Images
    public Sprite ActiveSprite = null;
    public Sprite InActiveSprite = null;

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
        // Sets the button up for use in store
        gameObject.name = us.name;
        _upgrade = us;

        // Set Title
        if (Title != null && TitleShadow != null)
        {
            Title.text = us.name;
            TitleShadow.text = us.name;
        }

        // Set Image
        if (PurchaseSprite != null)
        {
            if (us.icon != null)
                PurchaseSprite.sprite = us.icon;
            else
            {
                PurchaseSprite.sprite = null;
                Color c = PurchaseSprite.color;
                c.a = 0.0f;
                PurchaseSprite.color = c;
            }
        }

        // Set Description
        if (Description != null)
        {
            Description.text = us.description;
        }

        // Different Setup based on Store Button Type
        switch (us.type)
        {
            case UpgradeBoostGold.Gold:
                // Set Cost
                if (Cost != null && CostShadow != null)
                {
                    Cost.text = "$" + (us.MoneyCost / 100.0f).ToString();
                    CostShadow.text = "$" + (us.MoneyCost / 100.0f).ToString();
                    CoinImage.enabled = false;
                }
                // Make Sure Buy Button is active, no matter what
                if (BuyButton != null)
                {
                    BuyButton.SetActive(true);
                }
                // Switch off YouHave Text
                if (YouHave != null)
                {
                    YouHave.text = "";
                }
                // Make sure Active Button is off, no matter what
                if (ActiveButton != null)
                {
                    ActiveButton.SetActive(false);
                }
                break;
            case UpgradeBoostGold.Boost:
                // Set Cost
                if (Cost != null && CostShadow != null)
                {
                    Cost.text = us.CoinCost.ToString();
                    CostShadow.text = us.CoinCost.ToString();
                    CoinImage.enabled = true;
                }
                // Make Sure Buy Button is active, no matter what
                if (BuyButton != null)
                {
                    BuyButton.SetActive(true);
                }
                // Set YouHave Text
                if (YouHave != null)
                {
                    YouHave.text = "You Have: " + us.BoostsAvailable;
                }
                // Make sure Active Button is off, no matter what
                if (ActiveButton != null)
                {
                    ActiveButton.SetActive(false);
                }
                break;
            case UpgradeBoostGold.Upgrade:
                // Set Cost unless purchased
                if (Cost != null && CostShadow != null)
                {
                    if (us.Purchased)
                    {
                        Cost.text = "";
                        CostShadow.text = "";
                        CoinImage.enabled = false;
                    }
                    else
                    {
                        Cost.text = us.CoinCost.ToString();
                        CostShadow.text = us.CoinCost.ToString();
                        CoinImage.enabled = true;
                    }
                }
                // Make Sure Buy Button is active, unless purchased
                if (BuyButton != null)
                {
                    if (us.Purchased)
                    {
                        BuyButton.SetActive(false);
                    }
                    else
                    {
                        BuyButton.SetActive(true);
                    }
                }
                // Set YouHave Text to null
                if (YouHave != null)
                {
                    YouHave.text = "";
                }
                // Turn on Active button if active
                if (ActiveButton != null)
                {
                    if (us.Active)
                    {
                        ActiveButton.SetActive(true);
                        ActiveImage.sprite = ActiveSprite;
                    }
                    else if (us.Purchased)
                    {
                        ActiveButton.SetActive(true);
                        ActiveImage.sprite = InActiveSprite;
                    }
                    else
                    {
                        ActiveButton.SetActive(false);
                    }
                }
                break;
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
                                _storeSceneManager.UpdateButtons();
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
                                    if (us2.type == UpgradeBoostGold.Upgrade && us2.upgradeValues[0].upgradeType == us.upgradeValues[0].upgradeType && us2.Purchased && us2.Active)
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
                                bool active = us.Active;
                                foreach (UpgradeStruct us2 in _myGameManager._allUpgrades)
                                {
                                    if (us2.type == UpgradeBoostGold.Upgrade && us2.upgradeValues[0].upgradeType == us.upgradeValues[0].upgradeType && us2.Purchased && us2.Active)
                                    {
                                        us2.Active = false;
                                    }
                                }
                                us.Active = !active;
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
