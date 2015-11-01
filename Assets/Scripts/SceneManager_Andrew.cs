using UnityEngine;
using System.Collections;

public class SceneManager_Andrew : SceneManager_Base {
    public int m_Lives = 3;
    public GameObject m_PlayerObject = null;
    public GameObject m_GameOverText = null;
    public float m_Distance = 0;

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
            if (m_GameOverText != null)
            {
                m_GameOverText.SetActive(true);
            }
            Invoke("GameOver", 2.0f);
        }
    }

    public void GameOver()
    {
        if (_myGameManager != null)
        {
            _myGameManager.AddCoins(coinCount);
            _myGameManager.AddHighScore((int)m_Distance);
        }
        else Debug.Log("GameManager is null");
        Application.LoadLevel("Main Menu");
    }
}
