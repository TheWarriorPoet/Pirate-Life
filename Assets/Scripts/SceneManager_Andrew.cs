using UnityEngine;
using System.Collections;

public class SceneManager_Andrew : SceneManager_Base {
    public int m_Lives = 3;
    public GameObject m_PlayerObject = null;

    private static SceneManager_Andrew _instance = null;
    public static SceneManager_Andrew instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (SceneManager_Andrew)FindObjectOfType(typeof(SceneManager_Andrew));
            }
            return _instance;
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddLife(int a_Lives)
    {
        m_Lives += a_Lives;
    }

    public void Die()
    {
        m_Lives--;
        if (m_Lives == 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!!!");
        Time.timeScale = 0;
    }
}
