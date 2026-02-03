using UnityEngine;
using UnityEngine.UI;

public class DamCollecting : MonoBehaviour
{
    [Header("Progress")]
    public int collectedCount;
    public int requiredWood;
    public int currentDamStrength; // Acts as "Health" during the night

    [Header("References")]
    public PlayerHide playerHide;

    [Header("Visuals")]
    public Sprite[] damStages; // 0 = empty, last = complete
    public SpriteRenderer spriteRenderer;

    [Header("UI")]
    public Image progressFill; // UI Image (Fill type)

    public bool damFull = false;

    // --- SETUP METHODS (Called by GameplayDirector) ---

    // Called at the start of a new Day
    public void SetRequiredWood(int amount)
    {
        requiredWood = amount;
        ResetProgress(); // Start fresh for the day
    }

    // Called on Game Over or Day Reset
    public void ResetProgress()
    {
        collectedCount = 0;
        currentDamStrength = 0;
        damFull = false;
        UpdateVisuals();
    }

    // --- GAMEPLAY METHODS ---

    // Called by Wood Logs
    public void Collect()
    {
        if (collectedCount >= requiredWood) return; // dont update anymore once full
        collectedCount++;
        UpdateVisuals();

        if (collectedCount >= requiredWood) 
        {
            damFull = true;
        }
    }

    // Called by GameplayDirector when Night Starts
    public void FinalizeDefense()
    {
        // Your health is equal to the wood you collected
        currentDamStrength = collectedCount;
    }

    // Called by Wolves during Night
    public void TakeDamage(int damage)
    {
        if (currentDamStrength <= 0) return;

        currentDamStrength -= damage;
        
        // Also reduce collected count so the UI bar drops visibly
        collectedCount = currentDamStrength; 
        
        Debug.Log($"Dam under attack! Strength: {currentDamStrength}");
        UpdateVisuals();

        if (currentDamStrength <= 0)
        {
            BreakDam();
        }
    }

    void BreakDam()
    {
        Debug.Log("THE DAM BROKE!");
        
        // 1. Visually reset to 0
        ResetProgress();

        // 2. Force the player out of hiding so wolves can kill them
        if (playerHide != null)
        {
            playerHide.ForceUnhide(); 
        }
    }

    // --- VISUALS ---

    private void UpdateVisuals()
    {
        if (requiredWood == 0) return; // Prevent divide by zero errors

        // Calculate percentage
        float progress = (float)collectedCount / requiredWood;

        // 1. Sprite progress 
        if (damStages != null && damStages.Length > 0 && spriteRenderer != null)
        {
            int stageIndex = Mathf.Clamp(
                Mathf.FloorToInt(progress * (damStages.Length - 1)),
                0,
                damStages.Length - 1
            );
            spriteRenderer.sprite = damStages[stageIndex];
        }

        // 2. UI progress bar
        if (progressFill != null)
        {
            progressFill.fillAmount = progress;
        }
    }
}