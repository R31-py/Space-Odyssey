using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public static PlayerValues Instance;
    
    [SerializeField] public int health = 3;
    private int oldHealth = 3;
    [SerializeField] public int maxHealth;
    
    [SerializeField] public GameObject player;
    public ParticleSystem damageParticle;
    
    public int money = 100;
    public TextMeshProUGUI moneyText; // Assign this in Unity Inspector
    
    public KeyCode RIGHT = KeyCode.RightArrow;
    public KeyCode LEFT = KeyCode.LeftArrow;
    public KeyCode DASH = KeyCode.E;
    public KeyCode JUMP = KeyCode.Space;
    public KeyCode FIGHT = KeyCode.Z;

    public AbilityItem[] Inventory = new AbilityItem[3];
    
    // Tutorial Variables
    [SerializeField] public int tutorialStage = 0;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("A duplicate PlayerValues was found and destroyed.");
            Destroy(gameObject); 
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        oldHealth = health;
        Dictionary<string, object> savedData = PlayerSaveManager.LoadPlayerData();
        ApplyLoadedData(savedData);
    }

    void Update()
    {
        UpdateMoneyUI();

        if (oldHealth > health)
        {
            oldHealth = health;
        }

        if (health <= 0)
        {
            Debug.Log("Player Died! Reloading Save...");
            StartCoroutine(ReloadLastSave());
            money = 0;
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
    }

    private IEnumerator ReloadLastSave()
    {
        yield return new WaitForSeconds(2); // Optional delay before reload
        Dictionary<string, object> savedData = PlayerSaveManager.LoadPlayerData();
        ApplyLoadedData(savedData);
    }

    public void ApplyLoadedData(Dictionary<string, object> data)
    {
        health = (int)data["Health"];
        money = (int)data["Money"];
        player.transform.position = (Vector3)data["Position"];
    }
}
