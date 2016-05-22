using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager_Store : SceneManager_Base {
	public UnityEngine.UI.Text m_CoinCounter = null;
	public GameObject ButtonPrefab = null;
    public GameObject UpgradeButtonParent = null;
    public GameObject BoostsButtonParent = null;
    public GameObject GoldButtonParent = null;
    public ScrollRect ScrollView = null;
    private AudioClip soundEffect;

	private int coinCount;
    // Singleton Instance to provide simple access through other scripts
    private static SceneManager_Store _instance = null;
    public static SceneManager_Store instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (SceneManager_Store)FindObjectOfType(typeof(SceneManager_Store));
            }
            return _instance;
        }
    }
    // Use this for initialization
    void Start () {
		soundEffect = (AudioClip)Resources.Load("Sounds/store_buy");
        UpdateButtons();
        UpgradeButtonParent.SetActive(false);
        BoostsButtonParent.SetActive(false);
        GoldButtonParent.SetActive(true);
        ScrollView.content = GoldButtonParent.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		if (coinCount != _myGameManager.m_CoinScore)
		{
			coinCount = _myGameManager.m_CoinScore;

			if (m_CoinCounter != null && _myGameManager != null)
			{
				m_CoinCounter.text = "x " + _myGameManager.m_CoinScore;
			}
		}
	}

    public void UpdateButtons()
    {
        if (_myGameManager != null)
        {
            int TotalUpgradeCount = 0;
            int TotalBoostCount = 0;
            int TotalGoldCount = 0;
            foreach (UpgradeStruct us in _myGameManager._allUpgrades)
            {
                switch (us.type)
                {
                    case UpgradeBoostGold.Upgrade:
                        TotalUpgradeCount++;
                        break;
                    case UpgradeBoostGold.Boost:
                        TotalBoostCount++;
                        break;
                    case UpgradeBoostGold.Gold:
                        TotalGoldCount++;
                        break;
                }
            }
            UpgradeButtonParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotalUpgradeCount * 150);
            BoostsButtonParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotalBoostCount * 150);
            GoldButtonParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotalGoldCount * 150);
            int UpgradeCount = 0;
            int BoostCount = 0;
            int GoldCount = 0;
            foreach (UpgradeStruct us in _myGameManager._allUpgrades)
            {
                GameObject go;
                Vector2 newPosition = Vector2.zero;
                switch (us.type)
                {
                    case UpgradeBoostGold.Upgrade:
                        go = GameObject.Instantiate(ButtonPrefab, ButtonPrefab.transform.position, ButtonPrefab.transform.rotation) as GameObject;
                        go.GetComponent<Purchase>().SetupButton(us);
                        go.transform.SetParent(UpgradeButtonParent.transform, false);
                        newPosition.y = ((TotalUpgradeCount - 1) * 75) - (UpgradeCount++ * 150);
                        go.GetComponent<RectTransform>().anchoredPosition = newPosition;
                        break;
                    case UpgradeBoostGold.Boost:
                        go = GameObject.Instantiate(ButtonPrefab, ButtonPrefab.transform.position, ButtonPrefab.transform.rotation) as GameObject;
                        go.GetComponent<Purchase>().SetupButton(us);
                        go.transform.SetParent(BoostsButtonParent.transform, false);
                        newPosition.y = ((TotalBoostCount - 1) * 75) - (BoostCount++ * 150);
                        go.GetComponent<RectTransform>().anchoredPosition = newPosition;
                        break;
                    case UpgradeBoostGold.Gold:
                        go = GameObject.Instantiate(ButtonPrefab, ButtonPrefab.transform.position, ButtonPrefab.transform.rotation) as GameObject;
                        go.GetComponent<Purchase>().SetupButton(us);
                        go.transform.SetParent(GoldButtonParent.transform, false);
                        newPosition.y = ((TotalGoldCount - 1) * 75) - (GoldCount++ * 150);
                        go.GetComponent<RectTransform>().anchoredPosition = newPosition;
                        break;
                }
            }
        }
    }

    public void SwitchView(string view)
    {
        Debug.Log("SwitchView");
        UpgradeButtonParent.SetActive(false);
        BoostsButtonParent.SetActive(false);
        GoldButtonParent.SetActive(false);
        switch (view)
        {
            case "Upgrades":
                UpgradeButtonParent.SetActive(true);
                ScrollView.content = UpgradeButtonParent.GetComponent<RectTransform>();
                break;
            case "Boosts":
                BoostsButtonParent.SetActive(true);
                ScrollView.content = BoostsButtonParent.GetComponent<RectTransform>();
                break;
            case "Gold":
                GoldButtonParent.SetActive(true);
                ScrollView.content = GoldButtonParent.GetComponent<RectTransform>();
                break;
        }
    }

    public void Purchase(string UpgradeName)
    {
        if (_myGameManager != null)
        {
            foreach (UpgradeStruct us in _myGameManager._allUpgrades)
            {
                if (us.name == UpgradeName && _myGameManager.m_CoinScore >= us.CoinCost)
                {
                    _myGameManager.AddCoins(-us.CoinCost);
                    us.Active = true;
                }
            }
        }
    }

    public void Purchase()
    {
        Debug.Log("Purchase");
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
