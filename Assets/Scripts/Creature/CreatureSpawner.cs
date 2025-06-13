using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using UnityEngine.InputSystem;

public class CreatureSpawner : MonoBehaviour
{
    [Header("CreatureSpawner")]
    public static CreatureSpawner instance = null;

    [Header("Player")]
    public PlayerDamageable playerDamageable;

    [Header("Spawners")]
    public List<CreatureSpawn> creatureSpawners;

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

        if (playerDamageable == null)
        {
            playerDamageable = GameObject.Find("Player").GetComponent<PlayerDamageable>();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            creatureSpawners.Add(transform.GetChild(i).GetComponent<CreatureSpawn>());
        }
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
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

                creatureSpawned.GetComponent<BaseCreatureStateMachine>().spawnedByRegularLogic = true;
                creatureSpawned.GetComponent<Creature>().SetBreed(creatureSpawners[zone].GetRandomBreed());

                creatureSpawned.name = "" + creatureSpawned.GetComponent<Creature>().race;

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

    public void CreatureCaptured(GameObject capturedCreature)
    {
        creaturesAlive.Remove(capturedCreature);
        // creaturesAlive--;
        // CheckEnemiesLeft();
    }

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
