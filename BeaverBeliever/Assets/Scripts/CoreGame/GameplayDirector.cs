using UnityEngine;
using WorldTime; 
using System.Collections;

public class GameplayDirector : MonoBehaviour
{
    [Header("Scene References")]
    public WorldLight worldLight;
    public DamCollecting damCollector;
    public PlayerHide playerHide;
    public WolfSpawner packSpawner;
    public GameObject dayWolf;       
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip roosterClip;

    [Header("UI References")]
    public SlideUpPanel winMenuCanvas;

    [Header("Game Configuration")]
    public int day1duration = 20;
    public int day2duration = 45;
    public int day1RequiredWood = 4;
    public int day2RequiredWood = 8;

    private int currentDay = 1;

    private void Start()
    {
        WorldLight.OnNightStart += HandleNightStart; // Spawns Wolves
        WorldLight.OnDayCycleEnd += HandleMorning;   // Checks Survival
        if(winMenuCanvas) winMenuCanvas.gameObject.SetActive(false);
        SetupDay(1);
    }

    private void OnDestroy()
    {
        WorldLight.OnNightStart -= HandleNightStart;
        WorldLight.OnDayCycleEnd -= HandleMorning;
    }

    private void SetupDay(int dayIndex)
    {
        currentDay = dayIndex;
        
        // Audio
        if(audioSource && roosterClip) audioSource.PlayOneShot(roosterClip);

        // Rules
        if (currentDay == 1)
        {
            worldLight.dayDuration = day1duration;
            damCollector.SetRequiredWood(day1RequiredWood);
            if(dayWolf) dayWolf.SetActive(false);
        }
        else if (currentDay == 2)
        {
            worldLight.dayDuration = day2duration;
            damCollector.SetRequiredWood(day2RequiredWood);
            if(dayWolf) dayWolf.SetActive(true);
        }
        
        // Restore player ability to hide if they lost it previous night
        playerHide.ResetAbility(); 
        
        // Reset Clock
        worldLight.ResetDay();
    }

    // 1. NIGHT STARTS -> WOLVES SPAWN
    private void HandleNightStart()
    {
        Debug.Log("Night has fallen...");
        damCollector.FinalizeDefense(); // Sets Health = Collected Wood
        
        if (packSpawner) packSpawner.TriggerSpawn();
    }

    // 2. MORNING COMES -> DID WE SURVIVE?
    private void HandleMorning()
    {
        // If the code reaches here, the player is technically alive 
        // (because DieScript handles game over immediately on death).
        
        Debug.Log("Morning has broken.");
        
        // Cleanup Wolves
        foreach(var wolf in FindObjectsByType<WolfChase>(FindObjectsSortMode.None))
        {
            wolf.Retreat(); // <--- NEW LINE
        }

        if (currentDay == 1)
        {
            SetupDay(2);
        }
        else if (currentDay == 2)
        {
            TriggerWin();
        }
    }

    private void TriggerWin()
    {
        Debug.Log("Victory!");
        if(winMenuCanvas) winMenuCanvas.Show();
        Time.timeScale = 0f; 
    }
}