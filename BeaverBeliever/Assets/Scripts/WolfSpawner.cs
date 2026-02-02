
using UnityEngine;
using System.Collections;

public class WolfSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay = 30f;
    public float spawnInterval = 1f;
    public int maxEnemies = 10;

    private int spawned = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnDelay);

        while (spawned < maxEnemies)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
