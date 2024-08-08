using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave : MonoBehaviour
{
    [Header("Subwaves")]
    public List<Subwave> subwaves;
    public int currentSubWaveIndex;
    public float timeBetweenSubwaves;
    
    [Header("Spawnpoint")]
    public Vector3 spawnPosition;
    public float spawnRange;

    private void Awake() {
        spawnPosition = transform.position;
        currentSubWaveIndex = 0;
    }

    [SerializeField]
    public class Subwave
    {
        public List<GameObject> enemies;
        public float timeToNextEnemy;
    }
}
