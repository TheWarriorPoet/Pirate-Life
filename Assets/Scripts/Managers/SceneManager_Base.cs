using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityEngine.SceneManagement;

public class SceneManager_Base : MonoBehaviour {
    // public properties
    public GameObject gameManager = null;

    protected GameManager _myGameManager = null;


    // private properties

    //-------------------------------------------------------------------------------------
    // Awake()
    // Initialise the class
    //-------------------------------------------------------------------------------------
    public virtual void Awake()
    {
        _myGameManager = GameManager.instance;
        if (_myGameManager == null && gameManager != null)
        {
            Instantiate(gameManager);
            _myGameManager = GameManager.instance;
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void LoadScene(string a_SceneName)
    {
        Application.LoadLevel(a_SceneName);
        //SceneManager.LoadScene(a_SceneName);
    }

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ToggleSound()
	{
		AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
	}
}
