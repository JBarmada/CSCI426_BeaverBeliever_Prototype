using UnityEngine;
using System.Collections;

public class WolfSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1f;
    
    // We removed "maxEnemies" because we will pass it in as a parameter now

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip spawnSound;   

    private int spawned = 0;

    void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    // UPDATED: Now accepts 'count'
    public void TriggerSpawn(int count)
    {
        spawned = 0; 
        if (audioSource && spawnSound) audioSource.PlayOneShot(spawnSound);
        StartCoroutine(SpawnRoutine(count));
    }

    IEnumerator SpawnRoutine(int totalToSpawn)
    {
        while (spawned < totalToSpawn)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}