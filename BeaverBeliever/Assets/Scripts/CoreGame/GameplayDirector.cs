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
    
    [Header("Difficulty Balance")]
    public int day1RequiredWood = 4;
    public int day2RequiredWood = 8;
    public int day1WolfCount = 5;  
    public int day2WolfCount = 10; 

    [Header("Game Win Delay")]
    public float winDelaySeconds = 2f;

    private int currentDay = 1;

    private void Start()
    {
        WorldLight.OnNightStart += HandleNightStart; 
        WorldLight.OnDayCycleEnd += HandleMorning;   
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
        
        if(audioSource && roosterClip) audioSource.PlayOneShot(roosterClip);

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
        
        playerHide.ResetAbility(); 
        worldLight.ResetDay();
    }

    private void HandleNightStart()
    {
        Debug.Log("Night has fallen...");
        damCollector.FinalizeDefense(); 
        
        int wolvesToSpawn = (currentDay == 1) ? day1WolfCount : day2WolfCount;

        if (packSpawner) packSpawner.TriggerSpawn(wolvesToSpawn);
    }

    private void HandleMorning()
    {
        Debug.Log("Morning has broken.");
        
        // 1. Tell Wolves to Retreat immediately
        foreach(var wolf in FindObjectsByType<WolfChase>(FindObjectsSortMode.None))
        {
            wolf.Retreat(); 
        }

        // 2. Start the delay sequence
        StartCoroutine(MorningSequence());
    }

    private IEnumerator MorningSequence()
    {
        // Wait 3 seconds for wolves to run and light to change
        yield return new WaitForSeconds(winDelaySeconds);

        // Logic check: Did the player survive?
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