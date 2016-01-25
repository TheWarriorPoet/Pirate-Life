using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager_Store : SceneManager_Base {
	public UnityEngine.UI.Text m_CoinCounter = null;
	public Button MagneticButton = null;

	private int coinCount;

	// Use this for initialization
	void Start () {
		if (_myGameManager.magneticCoins)
		{
			MagneticButton.interactable = false;
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

    public void PurchaseMagneticCoin(int cost)
    {
        if(_myGameManager != null && _myGameManager.m_CoinScore >= cost)
        {
            _myGameManager.magneticCoins = true;
            _myGameManager.m_CoinScore -= cost;
            if (MagneticButton != null)
            {
                MagneticButton.interactable = false;
            }
        }
    }
}
