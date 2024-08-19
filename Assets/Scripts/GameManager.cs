using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [Header("Game")]
    bool isInGame;
    [SerializeField] InputAction startWave;
    [HideInInspector] public WaveSpawner waveSpawner;
    bool gameStarted;

    [Header("Menu")]
    bool optionsMenuIsOpen;
    public GameObject optionsMenu;
    [SerializeField] InputAction openMenu;
    [SerializeField] GameObject mainMenuSelectedFirst;
    [SerializeField] GameObject optionsMenuSelectedFirst;

    [Header("Scenes")]
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>(); 
    public GameObject loadScene;
    bool LoadingScene;

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

        isInGame = false;
        gameStarted = false;
        optionsMenuIsOpen = false;

        GetComponents();

        openMenu.Enable();
        openMenu.performed += context => OnOptions();

        EventSystem.current.SetSelectedGameObject(mainMenuSelectedFirst);
    }

    void GetComponents()
    {
        if(isInGame)
        {
            waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();
        }
        else
        {
            mainMenuSelectedFirst = GameObject.Find("Canvas").transform.Find("PlayButton").gameObject;
        }
        optionsMenu = GameObject.Find("Canvas").transform.Find("OptionsMenu").gameObject;
        optionsMenuSelectedFirst = optionsMenu.transform.GetChild(1).gameObject;
    }

    public void ButtonFunction(string button) {
        switch(button) 
        {
            case "start":
                scenesToLoad.Add(SceneManager.LoadSceneAsync("Game"));
                StartCoroutine(LoadingScreen());
            break;

            case "options":
                OnOptions();
            break;

            case "exit":
                QuitGame();
            break;

            default:
            break;
        }
    }

    IEnumerator LoadingScreen()
    {
        if(optionsMenuIsOpen)
        {
            OnOptions();
        }
        LoadingScene = true;

        float totalProgress = 0;

        for(int i = 0; i < scenesToLoad.Count; i++)
        {
            while(!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                yield return null;
            }
        }

        if(!isInGame)
        {
            isInGame = true;
            GetComponents();
            InitializeGame();
        }
        else
        {
            isInGame = false;
            GetComponents();
        }

        yield return new WaitForSeconds(0.01f);

        LoadingScene = false;
    }

    void InitializeGame()
    {
        startWave.Enable();
        startWave.performed += context => StartGame();
    }

    void StartGame()
    {
        if(!gameStarted)
        {
            StartCoroutine(waveSpawner.SpawnWave());
        }
    }

    public void OnOptions()
    {
        if(!LoadingScene)
        {
            if(!optionsMenuIsOpen)
            {
                optionsMenuIsOpen = true;
                optionsMenu.SetActive(optionsMenuIsOpen);
                EventSystem.current.SetSelectedGameObject(optionsMenuSelectedFirst);
                Time.timeScale = 0;
            }
            else
            {
                optionsMenuIsOpen = false;
                optionsMenu.SetActive(optionsMenuIsOpen);
                EventSystem.current.SetSelectedGameObject(mainMenuSelectedFirst);
                Time.timeScale = 1;
            }
        }
    }

    public void EndGame()
    {
        Debug.Log("Parabens vc terminou o jogo");
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

//alo
