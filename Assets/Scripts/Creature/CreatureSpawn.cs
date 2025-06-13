using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreatureSpawn : MonoBehaviour
{
    [Header("Spawnpoint")]
    public Vector3 spawnPosition;
    public float spawnRange;

    [Header("Creatures")]
    public List<CreatureSpawnRate> spawnRates = new List<CreatureSpawnRate>();

    private void Awake()
    {
        spawnPosition = transform.position;
    }

    [Serializable]
    public struct CreatureSpawnRate
    {
        public Creature.Breed creatureBreed;
        public int SpawnRatePorcentage;
    }

    public Creature.Breed GetRandomBreed()
    {
        List<Creature.Breed> spawnPool = new List<Creature.Breed>();

        foreach(CreatureSpawnRate spawnRate in spawnRates)
        {
            for (int i = 0; i < spawnRate.SpawnRatePorcentage; i++)
            {
                spawnPool.Add(spawnRate.creatureBreed);
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, spawnPool.Count);

        return spawnPool[randomIndex];
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 140, 0, 1);
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}