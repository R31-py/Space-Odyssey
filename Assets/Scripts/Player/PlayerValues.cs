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
    [SerializeField] private GameObject deathScreen;
    public static bool isDead = false;
    private Animator animator;
    public ParticleSystem damageParticle;
    public ParticleSystem deathParticle;
    
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
    
    // Invincibility
    [HideInInspector] public bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    
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
        animator = player.GetComponent<Animator>();
        //INVINCIBILITY 
        spriteRenderer = player.GetComponent<SpriteRenderer>();  // ← ktu
        Dictionary<string, object> savedData = PlayerSaveManager.LoadPlayerData();
        ApplyLoadedData(savedData);
    }


    void Update()
    {
        UpdateMoneyUI();

        if (oldHealth > health)
        {
            damageParticle.Play();
            Debug.Log(damageParticle);
            oldHealth = health;
            SoundManager.Instance.PlaySound2D("player_hurt");
        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            ParticleSystem particlesInstance = Instantiate(deathParticle, transform.position, Quaternion.identity);
            particlesInstance.Play();
            animator.Play("death");
            deathScreen.SetActive(true);
            money = 0;
        }

        if (!isDead)
        {
            deathScreen.SetActive(false);
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
    }

    public IEnumerator ReloadLastSave()
    {
        yield return new WaitForSeconds(0.5f); // Optional delay before reload
        Dictionary<string, object> savedData = PlayerSaveManager.LoadPlayerData();
        ApplyLoadedData(savedData);
        PlayerValues.isDead = false;
    }

    public void ApplyLoadedData(Dictionary<string, object> data)
    {
        health = 3;
        money = (int)data["Money"];
        player.transform.position = (Vector3)data["Position"];
    }
    
    //INVINCIBILITY CHANGES
    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible)
            StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        // visuelles Feedback: gelbe Farbe
        spriteRenderer.color = Color.yellow;

        yield return new WaitForSeconds(duration);

        // zurücksetzen
        spriteRenderer.color = Color.white;
        isInvincible = false;
    }

    
}
