using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [Header("Game")]
    bool isInGame;
    [SerializeField] InputAction startWave;
    [HideInInspector] public WaveSpawner waveSpawner;
    bool gameStarted;
    public GameObject screenMessage;
    public float messageDuration;

    [Header("Player")]
    PlayerInput playerInput;

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
            playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
            screenMessage = GameObject.Find("Canvas").transform.Find("ScreenMessage").gameObject;
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
        screenMessage.SetActive(false);
    }

    void StartGame()
    {
        if(!gameStarted)
        {
            StartCoroutine(waveSpawner.SpawnWave());
            gameStarted = true;
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

                if(isInGame)
                {
                    playerInput.actions.FindActionMap("Player").Disable();
                }
            }
            else
            {
                optionsMenuIsOpen = false;
                optionsMenu.SetActive(optionsMenuIsOpen);
                EventSystem.current.SetSelectedGameObject(mainMenuSelectedFirst);
                Time.timeScale = 1;

                if(isInGame)
                {
                    playerInput.actions.FindActionMap("Player").Enable();
                }
            }
        }
    }

    public IEnumerator EndGame(bool win)
    {
        if(win)
        {
            screenMessage.GetComponentInChildren<TMP_Text>().text = "VICTORY";
        }
        else
        {
            screenMessage.GetComponentInChildren<TMP_Text>().text = "DEFEAT";
        }
        screenMessage.SetActive(true);
        screenMessage.GetComponent<Animator>().SetBool("messageOn", true);

        yield return new WaitForSeconds(messageDuration);

        screenMessage.GetComponent<Animator>().SetBool("messageOn", false);

        yield return new WaitForSeconds(1);
        screenMessage.SetActive(false);

        scenesToLoad.Add(SceneManager.LoadSceneAsync("Menu"));
        StartCoroutine(LoadingScreen());
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
