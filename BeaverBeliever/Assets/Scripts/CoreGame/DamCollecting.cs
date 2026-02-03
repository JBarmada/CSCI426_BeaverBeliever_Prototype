using UnityEngine;
using UnityEngine.UI;

public class DamCollecting : MonoBehaviour
{
    [Header("Progress")]
    public int collectedCount;
    public int requiredWood;
    public int currentDamStrength;

    [Header("Balance Settings")]
    public int woodHealthMultiplier = 20; // 1 Wood Log = 20 HP

    [Header("References")]
    public PlayerHide playerHide;

    [Header("Visuals")]
    public Sprite[] damStages; 
    public SpriteRenderer spriteRenderer;

    [Header("UI")]
    public Image progressFill; 

    public bool damFull = false;

    // --- SETUP METHODS ---

    public void SetRequiredWood(int amount)
    {
        requiredWood = amount;
        ResetProgress(); 
    }

    public void ResetProgress()
    {
        collectedCount = 0;
        currentDamStrength = 0;
        damFull = false;
        UpdateVisuals();
    }

    // --- GAMEPLAY METHODS ---

    public void Collect()
    {
        if (collectedCount >= requiredWood) return; 
        collectedCount++;
        UpdateVisuals();

        if (collectedCount >= requiredWood) 
        {
            damFull = true;
        }
    }

    public void FinalizeDefense()
    {
        // MATH CHANGE: Wood * 20 = Total Health
        // Example: 4 Wood * 20 = 80 HP
        currentDamStrength = collectedCount * woodHealthMultiplier;
    }

    public void TakeDamage(int damage)
    {
        if (currentDamStrength <= 0) return;

        currentDamStrength -= damage;
        
        // MATH CHANGE: Convert Health back to "Wood Count" for the visual bar
        // We use CeilToInt so the bar drops gradually
        collectedCount = Mathf.CeilToInt((float)currentDamStrength / woodHealthMultiplier); 
        
        Debug.Log($"Dam Health: {currentDamStrength}");
        UpdateVisuals();

        if (currentDamStrength <= 0)
        {
            BreakDam();
        }
    }

    void BreakDam()
    {
        Debug.Log("THE DAM BROKE!");
        ResetProgress();
        if (playerHide != null) playerHide.ForceUnhide(); 
    }

    // --- VISUALS ---

    private void UpdateVisuals()
    {
        if (requiredWood == 0) return; 

        float progress = (float)collectedCount / requiredWood;

        if (damStages != null && damStages.Length > 0 && spriteRenderer != null)
        {
            int stageIndex = Mathf.Clamp(
                Mathf.FloorToInt(progress * (damStages.Length - 1)),
                0, damStages.Length - 1
            );
            spriteRenderer.sprite = damStages[stageIndex];
        }

        if (progressFill != null) progressFill.fillAmount = progress;
    }
}