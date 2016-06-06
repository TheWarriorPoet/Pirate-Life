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
    BackMaterial,
    HatMaterial,
    HatColour,
    FeatherMaterial,
    PantsMaterial,
    BodyMaterial,
    ParrotUpgrade
}

public enum UpgradeBoostGold
{
    Upgrade,
    Boost,
    Gold
}
[System.Serializable]
public struct UpgradeValue
{
    public UpgradeType upgradeType;
    public float value;
    public Color color;
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
    public string upgradeGOName;
    public bool Active;
    public bool Purchased;
    public int BoostsPerPurchase;
    public int BoostsAvailable;
    public float LastsFor;
    public int CoinCost;
    public int MoneyCost;
    public UpgradeBoostGold type;
    public Material upgradeMaterial;
}

[System.Serializable]
public class UpgradeSaveVersion
{
    public string name;
    //public List<UpgradeValue> upgradeValues;
    public bool Active;
    public bool Purchased;
    public int BoostsAvailable;
}

public class GameManager : MonoBehaviour
{
	public Player player;
    public int m_CoinScore;
    public List<HighScore> HighScores = new List<HighScore>();
    public List<UpgradeStruct> _allUpgrades = new List<UpgradeStruct>();
    public bool PlayerReset = false;
    public bool dataReset = false;
    public bool FirstPlay = true;
    public bool PlayTutorialSetting = false;
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
        Screen.SetResolution(576, 1024, false);
        if (_instance) DestroyImmediate(gameObject);
        else
            DontDestroyOnLoad(this);

        System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#if UNITY_EDITOR
        if (!dataReset)
            Load();
#else
        Load();
#endif
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
            temp.date = System.DateTime.Now.Date.ToString("dd\\/MM\\/yyyy");
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
                    temp.date = System.DateTime.Now.Date.ToString("dd\\/MM\\/yyyy");
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
        Debug.Log("Saving Data");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PL.dat");
        SaveData DataForSaving = new SaveData();
        foreach (HighScore hs in HighScores) { DataForSaving.SDHighScores.Add(hs); }
        foreach (UpgradeStruct us in _allUpgrades) {
            UpgradeSaveVersion USV = new UpgradeSaveVersion();
            USV.name = us.name;
            USV.Active = us.Active;
            USV.BoostsAvailable = us.BoostsAvailable;
            USV.Purchased = us.Purchased;
            //USV.upgradeValues = us.upgradeValues;
            DataForSaving.SDAllUpgrades.Add(USV);
        }
        DataForSaving.CoinScore = m_CoinScore;
        DataForSaving.FirstPlay = FirstPlay;
        DataForSaving.PlayTutorialSetting = PlayTutorialSetting;
        bf.Serialize(file, DataForSaving);
        file.Close();
        Debug.Log("Save Complete");
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
            FirstPlay = data.FirstPlay;
            PlayTutorialSetting = data.PlayTutorialSetting;
            foreach (UpgradeSaveVersion USV in data.SDAllUpgrades)
            {
                foreach (UpgradeStruct US in _allUpgrades)
                {
                    if (US.name == USV.name)
                    {
                        US.Active = USV.Active;
                        US.BoostsAvailable = USV.BoostsAvailable;
                        US.Purchased = USV.Purchased;
                        //US.upgradeValues = USV.upgradeValues;
                    }
                }
            }
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
    public List<UpgradeSaveVersion> SDAllUpgrades = new List<UpgradeSaveVersion>();
    public int CoinScore;
    public bool FirstPlay;
    public bool PlayTutorialSetting;
}
