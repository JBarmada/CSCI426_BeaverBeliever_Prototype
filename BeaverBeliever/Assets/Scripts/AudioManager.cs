using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Beaver")]
    public AudioClip beaverWalk;
    public AudioClip beaverChop;
    public AudioClip beaverHurt;
    public AudioClip beaverBuild;

    [Header("Wolves")]
    public AudioClip wolfSnarl;
    public AudioClip wolfRun;
    public AudioClip wolfBite;
    public AudioClip wolfDestroyDam;

    [Header("Environment")]
    public AudioClip treeHit;
    public AudioClip damJingle;
    public AudioClip riverLoop;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
