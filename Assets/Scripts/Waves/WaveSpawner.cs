using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WaveSpawner : MonoBehaviour
{

    [Header("Waves")]
    public List<Wave> waves;
    [HideInInspector] public int currentWaveIndex = 0;
    [HideInInspector] public bool waveSpawnEnded;

    [Header("Enemies")]
    public int enemiesAlive;

    [Header("Spawn")]
    public Pathfinding pathfinding;

    [Header("Canvas")]
    public GameObject waveClearText;
    public Animator waveClearAnimator;
    public float messageDuration;

    void Awake()
    {
        pathfinding = GameObject.Find("PathfindingManager").GetComponent<Pathfinding>();

        for(int i = 0; i < transform.childCount; i++)
        {
            waves.Add(transform.GetChild(i).GetComponent<Wave>());
        }
    }

    public IEnumerator SpawnWave()
    {
        waveSpawnEnded = false;
        enemiesAlive = 0;
        Wave currentWave = waves[currentWaveIndex];

        if(currentWave.subwaves == null || currentWave.subwaves.Count == 0)
        {
            Debug.Log("The wave " + currentWaveIndex + " doesnt have subwaves");
        }
        else
        {
            for(int i = 0; i < currentWave.subwaves.Count; i++)
            {
                int subwaveEnemiesAmount = currentWave.subwaves[currentWave.currentSubWaveIndex].enemies.Count;

                for(int o = 0; o < subwaveEnemiesAmount; o++)
                {
                    yield return new WaitForSeconds(currentWave.subwaves[currentWave.currentSubWaveIndex].timeToNextEnemy);
                    
                    GameObject enemySpawned = Instantiate(currentWave.subwaves[currentWave.currentSubWaveIndex].enemies[o], 
                    GetSpawnPosition(currentWave.spawnPosition, currentWave.spawnRange), 
                    Quaternion.identity, 
                    currentWave.transform);
                    
                    enemySpawned.GetComponent<BaseEnemyStateMachine>().spawnedInWave = true;
                    enemiesAlive++;
                }

                currentWave.currentSubWaveIndex++;

                yield return new WaitForSeconds(currentWave.timeBetweenSubwaves);
            }

            waveSpawnEnded = true;
        }
    }

    Vector3 GetSpawnPosition(Vector3 spawnPosition, float spawnRange)
    {
        Vector3 finalSpawnPosition = spawnPosition;

        finalSpawnPosition.x += Random.Range(-spawnRange, spawnRange);
        finalSpawnPosition.y += Random.Range(spawnRange, -spawnRange);
        
        if(pathfinding.ValidatePosition(finalSpawnPosition))
        {
            return finalSpawnPosition;
        }
        else
        {
            return GetSpawnPosition(spawnPosition, spawnRange);
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        CheckEnemiesLeft();
    }

    public void CheckEnemiesLeft()
    {
        if(enemiesAlive <= 0 && waveSpawnEnded)
        {
            currentWaveIndex++;
            if(currentWaveIndex == waves.Count)
            {
                StartCoroutine(GameManager.instance.EndGame(true));
            }
            else
            {
                StartCoroutine(WaveClear());
            }
        }
    }

    IEnumerator WaveClear()
    {
        waveClearText.GetComponentInChildren<TMP_Text>().text = "WAVE \n CLEAR";
        waveClearText.SetActive(true);
        waveClearAnimator.SetBool("messageOn", true);

        yield return new WaitForSeconds(messageDuration);

        waveClearAnimator.SetBool("messageOn", false);

        yield return new WaitForSeconds(1);
        waveClearText.SetActive(false);
        StartCoroutine(SpawnWave());
    }
}
