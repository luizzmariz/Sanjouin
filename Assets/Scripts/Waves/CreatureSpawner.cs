using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CreatureSpawner : MonoBehaviour
{
    [Header("Player")]
    public PlayerDamageable playerDamageable;

    [Header("Waves")]
    public List<CreatureSpawn> creatureSpawners;
    [HideInInspector] public int currentWaveIndex = 0;

    [Header("Enemies")]
    public GameObject creaturePrefab;
    public List<GameObject> creaturesAlive;

    [Header("Spawn")]
    public Pathfinding pathfinding;
    // [SerializeField] InputAction spawnEnemy;
    // public GameObject enemyToSpawn;
    // public Transform spawnLocation;
    public int creatureSpawnedLimit;

    [Header("Canvas")]
    public GameObject waveClearText;
    public Animator waveClearAnimator;
    public float messageDuration;

    // void OnEnable()
    // {
    //     spawnEnemy.Enable();
    //     spawnEnemy.performed += context => SpawnEnemy();
    // }

    // void OnDisable()
    // {
    //     spawnEnemy.Disable();
    // }

    void Awake()
    {
        pathfinding = GameObject.Find("PathfindingManager").GetComponent<Pathfinding>();
        if(playerDamageable == null)
        {
            playerDamageable = GameObject.Find("Player").GetComponent<PlayerDamageable>();
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            creatureSpawners.Add(transform.GetChild(i).GetComponent<CreatureSpawn>());
        }
    }

    // void SpawnEnemy()
    // {
    //     CreatureSpawn currentWave = creatureSpawners[currentWaveIndex];

    //     Instantiate(enemyToSpawn, 
    //                 GetSpawnPosition(spawnLocation.position, 0), 
    //                 Quaternion.identity, 
    //                 currentWave.transform);
    // }

    // void Update()
    // {
    //     foreach (CreatureSpawn spawner in creatureSpawners)
    //     {
            
    //     }
    // }

    public void SpawnEnemies()
    {
        if (creaturesAlive.Count < creatureSpawnedLimit)
        {
            for (int i = 0; i < creatureSpawnedLimit - creaturesAlive.Count; i++)
            {
                int zone = Random.Range(0, creatureSpawners.Count);

                GameObject creatureSpawned = Instantiate(creaturePrefab,
                GetSpawnPosition(creatureSpawners[zone].spawnPosition, creatureSpawners[zone].spawnRange),
                Quaternion.identity,
                creatureSpawners[zone].transform);

                creatureSpawned.GetComponent<BaseEnemyStateMachine>().spawnedInWave = true;
                creaturesAlive.Add(creatureSpawned);
            }
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
        // creaturesAlive--;
        // CheckEnemiesLeft();
    }

    // public void CheckEnemiesLeft()
    // {
    //     if(creaturesAlive <= 0)
    //     {
    //         currentWaveIndex++;
    //         if(currentWaveIndex == waves.Count)
    //         {
    //             StartCoroutine(GameManager.instance.EndGame(true));
    //         }
    //         else
    //         {
    //             StartCoroutine(WaveClear());
    //         }
    //     }
    // }

    IEnumerator WaveClear()
    {
        waveClearText.GetComponentInChildren<TMP_Text>().text = "WAVE \n CLEAR";
        waveClearText.SetActive(true);
        waveClearAnimator.SetBool("messageOn", true);

        playerDamageable.Heal(8);

        yield return new WaitForSeconds(messageDuration);

        waveClearAnimator.SetBool("messageOn", false);

        yield return new WaitForSeconds(1);
        waveClearText.SetActive(false);
        // StartCoroutine(SpawnWave());
    }
}
