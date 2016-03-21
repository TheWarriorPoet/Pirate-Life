using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager_Store : SceneManager_Base {
	public UnityEngine.UI.Text m_CoinCounter = null;
	public Button MagneticButton = null;

	private AudioClip soundEffect;

	private int coinCount;

	// Use this for initialization
	void Start () {
		soundEffect = (AudioClip)Resources.Load("Sounds/store_buy");
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
}
