using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager_HighScores : SceneManager_Base {
    public Text Dates = null;
    public Text Names = null;
    public Text Distances = null;
	// Use this for initialization
	void Start () {
        if (Dates != null && Names != null && Distances != null)
        {
            Dates.text = "DATES\n";
            Names.text = "NAMES\n";
            Distances.text = "DISTANCE\n";
            if (_myGameManager != null)
            {
                foreach (HighScore hs in _myGameManager.GetHighScores())
                {
                    Names.text += hs.name + "\n";
                    Dates.text += hs.date + "\n";
                    Distances.text += hs.distance + "m\n";
                }
            }
            else
            {
                Debug.Log("GameManager is null");
            }
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
