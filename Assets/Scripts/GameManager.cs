using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] InputAction startWave;
    [HideInInspector] public WaveSpawner waveSpawner;
    bool gameStarted;

    void Awake()
    {
        if(instance == null) 
        {
			instance = this;
		} 
        else if(instance != this) 
        {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

        GetComponents();
    }

    void GetComponents()
    {
        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();
    }

    void Start()
    {
        startWave.Enable();
        startWave.performed += context => StartGame();
    }

    void StartGame()
    {
        if(!gameStarted)
        {
            // StartCoroutine(waveSpawner.SpawnWave());
            Debug.Log("okay");
            gameStarted = true;
        }
    }
}

//alo
