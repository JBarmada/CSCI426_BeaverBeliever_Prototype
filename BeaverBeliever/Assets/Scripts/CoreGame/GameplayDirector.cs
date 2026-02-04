using UnityEngine;
using WorldTime; 
using System.Collections;
using Unity.VisualScripting;

public class GameplayDirector : MonoBehaviour
{
    [Header("Scene References")]
    public WorldLight worldLight;
    public DamCollecting damCollector;
    public PlayerHide playerHide;
    public WolfSpawner packSpawner;
    public GameObject dayWolf;       
    
    [Header("SFX Audio (One Shot)")]
    public AudioSource sfxSource; // Drag your existing AudioSource here
    public AudioClip roosterClip;
    public AudioClip winClip;

    [Header("Music Audio (Looping)")]
    public AudioSource musicSource; // ADD A NEW AudioSource for this!
    public AudioClip dayMusic;      // Drag Day Music here
    public AudioClip nightMusic;    // Drag Scary Night Music here

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
        // Ensure music loops
        if (musicSource) musicSource.loop = true;

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

        // 1. PLAY DAY MUSIC (Start of Day 1)
        PlayMusic(dayMusic);

        if (currentDay == 1)
        {
            worldLight.dayDuration = day1duration;
            damCollector.SetRequiredWood(day1RequiredWood);
            if(dayWolf) dayWolf.SetActive(false);
        }
        else if (currentDay == 2)
        {
            if (sfxSource && roosterClip) sfxSource.PlayOneShot(roosterClip);
            
            worldLight.dayDuration = day2duration;
            damCollector.SetRequiredWood(day2RequiredWood);
            damCollector.UpdateVisuals();
            if (dayWolf) dayWolf.SetActive(true);
            dayWolf.transform.localScale = new Vector3(4f, 4f, 0);
        }
        
        playerHide.ResetAbility(); 
        worldLight.ResetDay();
    }

    private void HandleNightStart()
    {
        Debug.Log("Night has fallen...");
        damCollector.FinalizeDefense(); 
        
        // 2. SWITCH TO SCARY MUSIC
        PlayMusic(nightMusic);

        int wolvesToSpawn = (currentDay == 1) ? day1WolfCount : day2WolfCount;
        if (packSpawner) packSpawner.TriggerSpawn(wolvesToSpawn);
    }

    private void HandleMorning()
    {
        Debug.Log("Morning has broken.");
        
        // 3. SWITCH BACK TO DAY MUSIC (Immediate relief)
        PlayMusic(dayMusic);

        // Tell Wolves to Retreat
        foreach(var wolf in FindObjectsByType<WolfChase>(FindObjectsSortMode.None))
        {
            wolf.Retreat(); 
        }

        StartCoroutine(MorningSequence());
    }

    private IEnumerator MorningSequence()
    {
        yield return new WaitForSeconds(winDelaySeconds);

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
        
        // Stop music so we can hear the win clip clearly
        if (musicSource) musicSource.Stop();
        if (sfxSource && winClip) sfxSource.PlayOneShot(winClip);

        if (winMenuCanvas) winMenuCanvas.Show();
        Time.timeScale = 0f; 
    }

    // --- HELPER TO SWAP TRACKS SMOOTHLY ---
    void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;

        // If this song is already playing, don't restart it!
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }
}