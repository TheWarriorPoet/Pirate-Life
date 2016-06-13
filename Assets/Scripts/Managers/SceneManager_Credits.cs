using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager_Credits : SceneManager_Base {
    public Toggle TutorialOn = null;
    private bool awaking = true;
    void Awake()
    {
        base.Awake();
        if (_myGameManager != null && TutorialOn != null)
        {
            TutorialOn.isOn = _myGameManager.FirstPlay;
        }
    }

    public void ToggleTutorial()
    {
        if (_myGameManager != null && TutorialOn != null)
        {
            _myGameManager.FirstPlay = TutorialOn.isOn;
        }
    }
}
