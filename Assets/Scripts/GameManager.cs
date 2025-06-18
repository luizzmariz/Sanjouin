using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [Header("Game")]
    bool isInGame;
    [HideInInspector] public CreatureSpawner creatureSpawner;
    public int messageDuration;

    [Header("Player")]
    PlayerInput playerInput;

    [Header("Menu")]
    bool optionsMenuIsOpen;
    public GameObject optionsMenu;
    [SerializeField] InputAction openMenu;
    [SerializeField] GameObject mainMenuSelectedFirst;
    [SerializeField] GameObject optionsMenuSelectedFirst;
    [SerializeField] GameObject howToPlayScreen;
    [SerializeField] GameObject howToPlaySelectedFirst;

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
        optionsMenuIsOpen = false;

        GetComponents();

        openMenu.Enable();
        openMenu.performed += context => OnOptions(context);

        if(Input.GetJoystickNames().Count() > 0 && Input.GetJoystickNames()[0] != "")
        {
            EventSystem.current.SetSelectedGameObject(mainMenuSelectedFirst);
        }
    }

    void GetComponents()
    {
        if(isInGame)
        {
            creatureSpawner = GameObject.Find("CreatureSpawner").GetComponent<CreatureSpawner>();
            playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        }
        else
        {
            mainMenuSelectedFirst = GameObject.Find("Canvas").transform.Find("PlayButton").gameObject;
            howToPlayScreen = GameObject.Find("Canvas").transform.Find("HowToPlayScreen").gameObject;

            howToPlaySelectedFirst = howToPlayScreen.transform.Find("Button").gameObject;
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

            case "howToPlay":
                HowToPlay();
            break;

            case "exit":
                QuitGame();
            break;

            default:
            break;
        }
    }

    public void OnOptions(InputAction.CallbackContext callbackContext)
    {
        if(!LoadingScene)
        {
            if(!optionsMenuIsOpen)
            {
                optionsMenuIsOpen = true;
                optionsMenu.SetActive(optionsMenuIsOpen);
                if(callbackContext.control.device.description.deviceClass != "Keyboard")
                {
                    EventSystem.current.SetSelectedGameObject(optionsMenuSelectedFirst);
                }
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
                if(callbackContext.control.device.description.deviceClass != "Keyboard")
                {
                    EventSystem.current.SetSelectedGameObject(mainMenuSelectedFirst);
                }
                Time.timeScale = 1;

                if(isInGame)
                {
                    playerInput.actions.FindActionMap("Player").Enable();
                }
            }
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
                if(Input.GetJoystickNames().Count() > 0 && Input.GetJoystickNames()[0] != "")
                {
                    EventSystem.current.SetSelectedGameObject(optionsMenuSelectedFirst);
                }
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
                if(Input.GetJoystickNames().Count() > 0 && Input.GetJoystickNames()[0] != "")
                {
                    EventSystem.current.SetSelectedGameObject(mainMenuSelectedFirst);
                }
                Time.timeScale = 1;

                if(isInGame)
                {
                    playerInput.actions.FindActionMap("Player").Enable();
                }
            }
        }
    }

    public void HowToPlay()
    {
        if(!howToPlayScreen.activeSelf)
        {
            howToPlayScreen.SetActive(true);
            if(Input.GetJoystickNames().Count() > 0 && Input.GetJoystickNames()[0] != "")
            {
                EventSystem.current.SetSelectedGameObject(howToPlaySelectedFirst);
            }
        }
        else
        {
            howToPlayScreen.SetActive(false);
            if(Input.GetJoystickNames().Count() > 0 && Input.GetJoystickNames()[0] != "")
            {
                EventSystem.current.SetSelectedGameObject(mainMenuSelectedFirst);
            }
        }
    }

    IEnumerator LoadingScreen()
    {
        openMenu.Disable();

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
            StartCoroutine(InitializeGame());
        }
        else
        {
            isInGame = false;
            GetComponents();
            openMenu.Enable();
        }

        yield return new WaitForSeconds(0.01f);


        LoadingScene = false;
    }

    IEnumerator InitializeGame()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        // screenMessage.GetComponentInChildren<TMP_Text>().text = "GAME STARTING IN 3";
        // screenMessage.SetActive(true);
        // screenMessage.GetComponent<Animator>().SetBool("messageOn", true);
        // int i = 3; 
        // while(i > 0)
        // {
        //     screenMessage.GetComponentInChildren<TMP_Text>().text = "GAME STARTING IN " + i;
        //     yield return new WaitForSeconds(0.75f);
        //     i--;
        // }
        playerInput.actions.FindActionMap("Player").Enable();
        openMenu.Enable();
        // screenMessage.GetComponent<Animator>().SetBool("messageOn", false);
        yield return new WaitForSeconds(0.1f);
        // screenMessage.SetActive(false);
        
        // StartCoroutine(creatureSpawner.SpawnWave());
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
