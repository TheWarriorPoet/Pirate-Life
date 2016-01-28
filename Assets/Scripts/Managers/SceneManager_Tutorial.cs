using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManager_Tutorial : SceneManager_Base {
    public Image ActiveScreen = null;
    public List<Sprite> Screens = new List<Sprite>();
    public GameObject PlayButton = null;
    public GameObject NextButton = null;
    public GameObject SkipButton = null;

    private int index = 0;
	// Use this for initialization
	void Start () {
        Next();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Skip()
    {
        SceneManager.LoadScene("Main Game");
    }

    public void Back()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Next()
    {
        if (ActiveScreen != null)
        {
            ActiveScreen.sprite = Screens[index++];
            Debug.Log("Index: " + index);
            Debug.Log("Count: " + Screens.Count);
            if (index >= Screens.Count)
            {
                LastScreen();
            }
        }
    }
    public void LastScreen()
    {
        if (PlayButton != null && NextButton != null && SkipButton != null)
        {
            NextButton.SetActive(false);
            PlayButton.SetActive(true);
            SkipButton.SetActive(false);
        }
    }
}
