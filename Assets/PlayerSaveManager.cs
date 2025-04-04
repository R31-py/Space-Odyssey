using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSaveManager : MonoBehaviour
{
    public static PlayerSaveManager Instance;
    public static class SaveLoadState
    {
        public static bool loadingFromSave = false;
    }
    private PlayerValues playerValues;
    private string sceneName;

    private static Dictionary<string, bool> bossesDefeated = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerValues = PlayerValues.Instance;
        if (playerValues == null)
        {
            Debug.LogError("PlayerValues instance not found!");
            return;
        }

        LoadPlayerData();
    }

    public void SavePlayerData(string sceneName)
    {
        if (playerValues == null) return;

        PlayerPrefs.SetInt("Health", playerValues.health);
        PlayerPrefs.SetInt("Money", playerValues.money);
        
        PlayerPrefs.SetFloat("PlayerX", playerValues.player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", playerValues.player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", playerValues.player.transform.position.z);
        
        PlayerPrefs.SetString("LastScene", sceneName);

        foreach (var boss in bossesDefeated)
        {
            PlayerPrefs.SetInt("Boss_" + boss.Key, boss.Value ? 1 : 0);
        }

        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    public static Dictionary<string, object> LoadPlayerData()
    {
        Dictionary<string, object> loadedData = new Dictionary<string, object>();

        loadedData["Health"] = PlayerPrefs.GetInt("Health", 3);
        loadedData["Money"] = PlayerPrefs.GetInt("Money", 100);

        float x = PlayerPrefs.GetFloat("PlayerX", 0);
        float y = PlayerPrefs.GetFloat("PlayerY", 0);
        float z = PlayerPrefs.GetFloat("PlayerZ", 0);
        loadedData["Position"] = new Vector3(x, y, z);

        loadedData["LastScene"] = PlayerPrefs.GetString("LastScene", SceneManager.GetActiveScene().name);

        // Load Bosses Defeated
        Dictionary<string, bool> loadedBosses = new Dictionary<string, bool>();
        foreach (string bossName in new string[] { "Robotut", "Boss2", "Boss3" })
        {
            loadedBosses[bossName] = PlayerPrefs.GetInt("Boss_" + bossName, 0) == 1;
        }
        loadedData["BossesDefeated"] = loadedBosses;

        Debug.Log("Game Loaded!");
        return loadedData;
    }

    public void SetBossDefeated(string bossName)
    {
        bossesDefeated[bossName] = true;
    }

    public bool IsBossDefeated(string bossName)
    {
        return bossesDefeated.ContainsKey(bossName) && bossesDefeated[bossName];
    }
}
