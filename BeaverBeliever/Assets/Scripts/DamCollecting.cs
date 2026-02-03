using UnityEngine;
using UnityEngine.UI;

public class DamCollecting : MonoBehaviour
{
    [Header("Progress")]
    public int collectedCount;
    public int requiredWood;

    [Header("Visuals")]
    public Sprite[] damStages; // 0 = empty, last = complete
    public SpriteRenderer spriteRenderer;

    [Header("UI")]
    public Image progressFill; // UI Image (Fill type)


    private void OnEnable()
    {
        WorldTime.WorldLight.OnDayStarted += HandleNewDay;
    }

    private void OnDisable()
    {
        WorldTime.WorldLight.OnDayStarted -= HandleNewDay;
    }

    private void HandleNewDay(int day)
    {
        if (day == 1)
            SetRequiredWood(4);
        else if (day == 2)
            SetRequiredWood(8);
    }
    public void SetRequiredWood(int amount)
    {
        requiredWood = amount;
        collectedCount = 0;
        UpdateVisuals();
    }

    public void Collect()
    {
        collectedCount++;
        UpdateVisuals();

        if (collectedCount >= requiredWood)
        {
            OnDamCompleted();
        }
    }

    private void UpdateVisuals()
    {
        // Sprite progress
        float progress = (float)collectedCount / requiredWood;
        int stageIndex = Mathf.Clamp(
            Mathf.FloorToInt(progress * damStages.Length),
            0,
            damStages.Length - 1
        );

        //spriteRenderer.sprite = damStages[stageIndex];

        // UI progress bar
        if (progressFill != null)
            progressFill.fillAmount = progress;
    }

    private void OnDamCompleted()
    {
        Debug.Log("Dam completed!");
        // Trigger victory, sound, animation, etc
    }
}
