using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public int spawnCount;
    public float x;
    public float y;
    public float z;
    public float minDistance;
    public float spawnDelay;

    public float bossX;
    public float bossY;
    public float bossZ;

    public AudioSource spawningAudio;

    private int remainingEnemies;

    void Start()
    {
        remainingEnemies = spawnCount;
        StartCoroutine(SpawnEnemies());
        spawningAudio.enabled = true;
    }

    IEnumerator SpawnEnemies()
    {
        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 position;
            bool validPosition;

            do
            {
                position = new Vector3(Random.Range(-x, x), y, Random.Range(-z, z));
                validPosition = true;

                foreach (Vector3 usedPosition in usedPositions)
                {
                    if (Vector3.Distance(position, usedPosition) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            usedPositions.Add(position);
            GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            GeneralHealth enemyHealth = newEnemy.GetComponent<GeneralHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += HandleEnemyDeath;
            }
            spawningAudio.Play();

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void HandleEnemyDeath()
    {
        remainingEnemies--;
        Debug.Log("ENEMIES REMAINING" + remainingEnemies);

        if (remainingEnemies <= 0)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        Debug.Log("SPAWNING BOSS");
        Vector3 bossPosition = new Vector3(bossX, bossY, bossZ);
        Instantiate(bossPrefab, bossPosition, Quaternion.identity);
    }
}
