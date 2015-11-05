using UnityEngine;
using System.Collections;

public class SceneManager_Andrew : SceneManager_Base {
    public int m_Lives = 3;
    public GameObject m_PlayerObject = null;
    public GameObject m_GameOverText = null;
    public float m_Distance = 0;

    public UnityEngine.UI.Text m_CoinCounter = null;
    public int coinCount = 0;

    public bool magneticCoins = false;

	public UnityEngine.UI.Text drunkMeter;

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

    new void Awake()
    {
        base.Awake();
        if (_myGameManager != null && _myGameManager.magneticCoins)
        {
            magneticCoins = true;
        }
    }
    // Use this for initialization
    void Start()
	{
	    
	}
	
	// Update is called once per frame
	void Update()
	{
		// GUI
		Player p = m_PlayerObject.GetComponent<Player>();

		drunkMeter.text = p.drunkenness.ToString() + "% Drunk";
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

    public void AddCoins(int a_iNumberOfCoins)
    {
        coinCount += a_iNumberOfCoins;
        if (m_CoinCounter != null)
        {
            m_CoinCounter.text = "Coins: " + coinCount;
        }
    }
}
