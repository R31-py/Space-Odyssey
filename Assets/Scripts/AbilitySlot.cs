using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public Image abilityIcon;
    public float cooldownTime = 5f;
    private bool isOnCooldown = false;
    [SerializeField] private int index;
    [SerializeField] public Ability ability;

    private void Awake()
    {
    }

    private void UseAbility()
    {
        if (!isOnCooldown)
        {
            // Trigger the ability's action
            Debug.Log("Ability used : " + index);
            ability.Attack();
            // Start cooldown
            StartCoroutine(Cooldown());
        }
    }
    
    private void Update()
    {
        // Input.GetKey uses UnityEngine.Input now (no need for the Windows-specific namespace)
        if (Input.GetKey(KeyCode.Z) && index == 1)
        {
            UseAbility();
        }
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        // Disable the ability (dim the icon, etc.)
        //abilityIcon.color = new Color(1, 1, 1, 0.5f); // Make the icon semi-transparent
        yield return new WaitForSeconds(cooldownTime);
        // Re-enable the ability
        //abilityIcon.color = new Color(1, 1, 1, 1f);  // Reset icon opacity
        isOnCooldown = false;
    }
}