using UnityEngine;
using System.Collections;

public class SceneManager_MainMenu : SceneManager_Base {
    public UnityEngine.UI.Text m_CoinCounter = null;

    // Use this for initialization
    void Start () {
	    if (m_CoinCounter != null && _myGameManager != null)
        {
            m_CoinCounter.text = "x " + _myGameManager.m_CoinScore;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
