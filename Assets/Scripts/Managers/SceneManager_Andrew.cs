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

	public UnityEngine.UI.Text drunkText;
	public GameObject drunkMask;
	private RectTransform drunkRect;
	private float drunkWidth;

    public GameObject[] hearts;

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
		drunkRect = drunkMask.GetComponent<RectTransform>();
		drunkWidth = drunkRect.sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update()
	{
		// GUI
		Player p = m_PlayerObject.GetComponent<Player>();

		// Drunk Meter
		drunkText.text = p.drunkenness.ToString() + "% Drunk";

		Vector2 newSize;

		newSize = drunkRect.sizeDelta;
		newSize.x = drunkWidth * p.drunkenness / 100.0f;
		drunkRect.sizeDelta = newSize;

		Vector2 newPos;

		newPos = drunkRect.localPosition;
		newPos.x = -drunkWidth * (1.0f - p.drunkenness / 100.0f) / 2;
		drunkRect.localPosition = newPos;

		// Lives
		for (int i = 0; i < hearts.Length; i++)
        {
            if (m_Lives >= i + 1 && hearts[i].activeInHierarchy)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
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
            m_CoinCounter.text = "x " + coinCount;
        }
    }
}
