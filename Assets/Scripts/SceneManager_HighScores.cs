using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager_HighScores : SceneManager_Base {
    public Text HighScores = null;
	// Use this for initialization
	void Start () {
        HighScores.text = "";
        if (_myGameManager != null)
        {
            foreach (HighScore hs in _myGameManager.GetHighScores())
            {
                HighScores.text += hs.name + "\t\t" + hs.date + "\t\t" + hs.distance + "m\n";
            }
        }
        else
        {
            Debug.Log("GameManager is null");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MainMenu()
    {
        Application.LoadLevel("Main Menu");
    }
}
