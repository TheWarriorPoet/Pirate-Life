using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public enum UpgradeType
{
    NullValue,
    CoinAttractRange,
    CoinAttractSpeed,
}
[System.Serializable]
public struct UpgradeValue
{
    public UpgradeType upgradeType;
    public float value;
}

[System.Serializable]
public struct HighScore
{
    public string name;
    public int distance;
    public string date;
}

[System.Serializable]
public class UpgradeStruct
{
    public string name;
    public string description;
    public Sprite icon;
    public List<UpgradeValue> upgradeValues;
    public Upgrade upgradeScript;
    public bool Active;
    public int BoostsAvailable;
    public float LastsFor;
    public int CoinCost;
}

public class GameManager : MonoBehaviour
{
	public Player player;
    public int m_CoinScore;
    public List<HighScore> HighScores = new List<HighScore>();
    public List<UpgradeStruct> _allUpgrades = new List<UpgradeStruct>();
    public bool PlayerReset = false;
    /// <summary>
    /// This needs to be connected once we have new track sections generating. 
    /// Whenever a new section of track is deployed, some upgrades need to reset their lists of affected objects
    /// </summary>
    public bool newTrackSection = false;

    // Singleton Instance to provide simple access through other scripts
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
            }
            return _instance;
        }
    }
    void Awake()
    {
        //System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        DontDestroyOnLoad(this);
        Load();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		// Quit from any scene anytime
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

    public void AddCoins(int a_iNumberOfCoins)
    {
        m_CoinScore += a_iNumberOfCoins;
    }

    public List<HighScore> GetHighScores()
    {
        HighScores.Sort((s1, s2) => s2.distance.CompareTo(s1.distance));
        //HighScores.Reverse();
        return HighScores;
    }

    public void AddHighScore(int a_distanceScore)
    {
        if (HighScores.Count < 10)
        {
            HighScore temp = new HighScore();
            temp.distance = a_distanceScore;
            temp.date = System.DateTime.Now.ToString("dd\\/MM\\/yyyy h\\:mm tt");
            temp.name = "Default";
            HighScores.Add(temp);
            HighScores.Sort((s1, s2) => s2.distance.CompareTo(s1.distance));
            HighScores.Reverse();
        }
        else
        {
            HighScores.Sort((s1, s2) => s2.distance.CompareTo(s1.distance));
            HighScores.Reverse();
            for (int i = 0; i < HighScores.Count; ++i)
            {
                if (HighScores[i].distance > a_distanceScore)
                {
                    HighScore temp = new HighScore();
                    temp.distance = a_distanceScore;
                    temp.date = System.DateTime.Now.ToString("dd\\/MM\\/yyyy h\\:mm tt");
                    temp.name = "Default";
                    HighScores[i] = temp;
                    return;
                }
            }
        }
    }

    void OnDisable()
    {
        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PL.dat");
        SaveData DataForSaving = new SaveData();
        foreach (HighScore hs in HighScores) { DataForSaving.SDHighScores.Add(hs); }
        DataForSaving.CoinScore = m_CoinScore;
        bf.Serialize(file, DataForSaving);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/PL.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PL.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            HighScores = data.SDHighScores;
            m_CoinScore = data.CoinScore;
        }
        else
        {
            m_CoinScore = 0;
        }
    }

    public float ReturnHighestUpgradeValue(UpgradeType upgradeType)
    {
        float returnValue = 0.0f;
        foreach (UpgradeStruct us in _allUpgrades)
        {
            foreach (UpgradeValue uv in us.upgradeValues)
            {
                if (uv.upgradeType == upgradeType && uv.value > returnValue)
                {
                    returnValue = uv.value;
                }
            }
        }
        return returnValue;
    }

    public float ReturnLowestUpgradeValue(UpgradeType upgradeType)
    {
        float returnValue = 0.0f;
        foreach (UpgradeStruct us in _allUpgrades)
        {
            foreach (UpgradeValue uv in us.upgradeValues)
            {
                if (uv.upgradeType == upgradeType && uv.value < returnValue)
                {
                    returnValue = uv.value;
                }
            }
        }
        return returnValue;
    }
}

[System.Serializable]
class SaveData
{
    public List<HighScore> SDHighScores = new List<HighScore>();
    public int CoinScore;
}
