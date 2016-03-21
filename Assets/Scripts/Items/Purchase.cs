using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Purchase : MonoBehaviour {
    private GameManager _myGameManager = null;
    public Text Title = null;
    public Text TitleShadow = null;
    public Text Cost = null;
    public Text CostShadow = null;
	// Use this for initialization
	void Start () {
        _myGameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetupButton(UpgradeStruct us)
    {
        gameObject.name = us.name;
        if (Title != null && TitleShadow != null)
        {
            Title.text = us.name;
            TitleShadow.text = us.name;
        }
        if (Cost != null && CostShadow != null)
        {
            Cost.text = us.CoinCost.ToString();
            CostShadow.text = us.CoinCost.ToString();
        }
    }

    public void PurchaseUpgrade()
    {
        if (_myGameManager != null)
        {
            foreach (UpgradeStruct us in _myGameManager._allUpgrades)
            {
                if (us.name == gameObject.name && _myGameManager.m_CoinScore >= us.CoinCost)
                {
                    _myGameManager.AddCoins(-us.CoinCost);
                    us.Active = true;
                }
            }
        }
    }
}
