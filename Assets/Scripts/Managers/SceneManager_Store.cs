using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager_Store : SceneManager_Base {
	public UnityEngine.UI.Text m_CoinCounter = null;
	public GameObject ButtonPrefab = null;
    public GameObject Parent = null;
	private AudioClip soundEffect;

	private int coinCount;
     
	// Use this for initialization
	void Start () {
		soundEffect = (AudioClip)Resources.Load("Sounds/store_buy");
        if (_myGameManager != null)
        {
            int Count = 0;
            Parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _myGameManager._allUpgrades.Count * 150);
            foreach (UpgradeStruct us in _myGameManager._allUpgrades)
            {
                GameObject go = GameObject.Instantiate(ButtonPrefab, ButtonPrefab.transform.position, ButtonPrefab.transform.rotation) as GameObject;
                go.GetComponent<Purchase>().SetupButton(us);
                go.transform.SetParent(Parent.transform, false);
                Vector2 newPosition = Vector2.zero;
                newPosition.y = ((_myGameManager._allUpgrades.Count - 1) * 75) - (Count * 150);
                go.GetComponent<RectTransform>().anchoredPosition = newPosition;
                Count++;
            }
        }
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
