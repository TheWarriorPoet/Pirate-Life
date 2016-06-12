using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager_Andrew : SceneManager_Base {
    public int m_Lives = 1;
    public GameObject m_PlayerObject = null;
    public GameObject m_GameOverText = null;
    public float m_Distance = 0;
	public UnityEngine.UI.Text m_DistanceCounter = null;

	public UnityEngine.UI.Text m_CoinCounter = null;
    public int coinCount = 0;
    public int coinMultiplier = 1;

    public UnityEngine.UI.Text drunkText;
	public GameObject drunkMask;
	private RectTransform drunkRect;
	private float drunkWidth;

    public GameObject[] hearts;

    //Difficulty Increase
    public float DifficultyIncreaseTimer = 30.0f;
    public float DifficultyMultiplier = 1.1f;
    private Player GamePlayer = null;

    //Pause
    public GameObject PauseMenu = null;

    //Game Over Menu
    public GameObject GameOverMenu = null;
    public Text DistanceText = null;
    public Text DistanceTextShadow = null;

    private UpgradeManager _upgradeManager = null;

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
    }
    // Use this for initialization
    void Start()
	{
		drunkRect = drunkMask.GetComponent<RectTransform>();
		drunkWidth = drunkRect.sizeDelta.x;
        GamePlayer = Player.instance;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
        if (GameOverMenu != null)
        {
            GameOverMenu.SetActive(false);
        }
        _upgradeManager = UpgradeManager.instance;
        if (_upgradeManager != null)
        {
            _upgradeManager.DecreaseActiveBoosts();
        }
        if (_myGameManager != null)
        {
            _myGameManager.FirstPlay = false;
        }
        StartCoroutine("DifficultyCoroutine");
	}

    IEnumerator DifficultyCoroutine()
    {
        float timer = 0.0f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= DifficultyIncreaseTimer)
            {
                timer = 0.0f;
                GamePlayer.multiplier *= DifficultyMultiplier;
                Debug.Log("Difficulty Increased");
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
	{
		// GUI
		//Player p = m_PlayerObject.GetComponent<Player>();

		// Drunk Meter
		drunkText.text = GamePlayer.drunkenness.ToString() + "% Drunk";

		Vector2 newSize;

		newSize = drunkRect.sizeDelta;
		newSize.x = drunkWidth * GamePlayer.drunkenness / 100.0f;
		drunkRect.sizeDelta = newSize;

		Vector2 newPos;

		newPos = drunkRect.localPosition;
		newPos.x = -drunkWidth * (1.0f - GamePlayer.drunkenness / 100.0f) / 2;
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

        foreach (UpgradeStruct u in _myGameManager._allUpgrades)
        {
            if (u.upgradeScript != null)
                u.upgradeScript.UpgradeUpdate();
        }

		// Distance
		if (m_DistanceCounter != null)
		{
			m_DistanceCounter.text = (int) m_Distance + "m";
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
            /*if (m_GameOverText != null)
            {
                m_GameOverText.SetActive(true);
            }
            Invoke("GameOver", 2.0f);*/
            GameOver();
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
        if (GameOverMenu != null)
        {
            GameOverMenu.SetActive(true);
            if (DistanceText != null) DistanceText.text = (int)m_Distance + "m";
            if (DistanceTextShadow != null) DistanceTextShadow.text = (int)m_Distance + "m";
        }
    }

    public void LoadMainMenu()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
        //Application.LoadLevel("Main Menu");
        SceneManager.LoadScene("Main Menu");
    }

    public void AddCoins(int a_iNumberOfCoins)
    {
        coinCount += a_iNumberOfCoins;
        if (m_CoinCounter != null)
        {
            m_CoinCounter.text = "x " + coinCount;
        }
    }

	public void TogglePause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
        }
	}

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Game");
    }

	public void BuyLife()
	{
		int cost = 200; // temp

		// Check gamemanager for gold, deduct gold if enough
		if (_myGameManager.m_CoinScore >= cost)
		{
			_myGameManager.m_CoinScore -= cost;

			// Add new life to counter
			m_Lives++;

			// Reset character
			GamePlayer.ResetCharacter();

			// Hide gameover window
			GameOverMenu.SetActive(false);

			// Save
			_myGameManager.Save();
		}
	}
}
