using UnityEngine;
using System.Collections;

public class SceneManager_MainMenu : SceneManager_Base {

	// Use this for initialization
	void Start () {
	    if (m_CoinCounter != null && _myGameManager != null)
        {
            m_CoinCounter.text = "Coins: " + _myGameManager.m_CoinScore;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NewGame()
    {
        Application.LoadLevel("Main Game");
    }

    public void HighScores()
    {
        Application.LoadLevel("High Scores");
    }

    public void Credits()
    {
        Application.LoadLevel("Credits");
    }
}
