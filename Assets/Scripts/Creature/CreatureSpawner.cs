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
        // StartCoroutine(Teste());
    }

    // IEnumerator Teste()
    // {
    //     if (creaturesAlive.Count < creatureSpawnedLimit)
    //     {
    //         for (int i = 0; i < creatureSpawnedLimit - creaturesAlive.Count; i++)
    //         {
    //             int zone = Random.Range(0, creatureSpawners.Count);

    //             GameObject creatureSpawned = Instantiate(creaturePrefab,
    //             GetSpawnPosition(creatureSpawners[zone].spawnPosition, creatureSpawners[zone].spawnRange),
    //             Quaternion.identity,
    //             creatureSpawners[zone].transform);

    //             creatureSpawned.GetComponent<BaseCreatureStateMachine>().spawnedByRegularLogic = true;
    //             creatureSpawned.GetComponent<Creature>().SetBreed(creatureSpawners[zone].GetRandomBreed());

    //             creatureSpawned.name = "" + creatureSpawned.GetComponent<Creature>().race;

    //             creaturesAlive.Add(creatureSpawned);
    //             yield return new WaitForSeconds(4f);
    //         }
    //     }
    // }

    Vector3 GetSpawnPosition(Vector3 spawnPosition, float spawnRange)
    {
        Vector3 finalSpawnPosition = spawnPosition;

        finalSpawnPosition.x += Random.Range(-spawnRange, spawnRange);
        finalSpawnPosition.y += Random.Range(spawnRange, -spawnRange);

        if (pathfinding.ValidatePosition(finalSpawnPosition))
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

    public List<GameObject> GetAllCreaturesAlive()
    {
        return creaturesAlive;
    }
}
