using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager_Store : SceneManager_Base {

    public Button MagneticButton = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
