using UnityEngine;
using System.Collections;

public class WolfSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1f;
    public int maxEnemies = 10;

    [Header("Audio")]
    public AudioSource audioSource; // Drag AudioSource here
    public AudioClip spawnSound;    // Drag Wolf Howl clip here

    private int spawned = 0;

    // Called by GameplayDirector at night
    public void TriggerSpawn()
    {
        spawned = 0; 
        
        // Play the sound once when the pack arrives
        if (audioSource && spawnSound)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (spawned < maxEnemies)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}