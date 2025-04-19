using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        START_GAME,
        PLAY_GAME,
        PAUSE_GAME,
        GAME_OVER,
    }
    [SerializeField] GameObject player;
    [SerializeField] private NavMeshSurface Surface2D;
    [SerializeField] private GameObject generator;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject wonUI;
    [SerializeField] private GameObject deadUI;
    [SerializeField] private GameObject continueUI;

    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;

    [SerializeField] private string sceneToLoad;

    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private BoolVariable loadedGame;
    [SerializeField] private PlayerStateVariable playerStateVariable;

    [SerializeField] private VoidEvent destroyAllEnemies;
    [SerializeField] private VoidEvent gameLoadedEvent;

    private GameState state = GameState.START_GAME;
    private RoomComplete roomComplete;
    private bool gamePaused = false;
    private bool options = false;

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Surface2D.BuildNavMeshAsync();
        roomComplete = generator.GetComponent<RoomComplete>();
        if (loadedGame.value == true)
        {
            LoadSavedState();
        }
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.START_GAME:
                loadingUI.SetActive(true);
                playerUI.SetActive(false);
                pauseUI.SetActive(false);
                if (Surface2D.isActiveAndEnabled && roomComplete.generationIsComplete)
                {
                    player.transform.position = GameObject.Find("SpawnPoint").transform.position;
                    loadingUI.SetActive(false);
                    playerUI.SetActive(true);
                    playerHealth.value = 6;
                    state = GameState.PLAY_GAME;
                }
                break;
            case GameState.PLAY_GAME:
                Time.timeScale = 1;
                break;
            case GameState.PAUSE_GAME:
                Time.timeScale = 0;
                break;
            case GameState.GAME_OVER:
                Time.timeScale = 0;
                break;
        }
    }

    public void MainMenu()
    {
        sceneToLoad = "MainMenu";
        pauseUI.SetActive(false);
        playerUI.SetActive(false);
        loadingUI.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
        StartCoroutine(LoadSceneASync());
    }

    public void ResumeBtn()
    {
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        playerUI.SetActive(true);
        gamePaused = false;
        state = GameState.PLAY_GAME;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!gamePaused)
            {
                pauseUI.SetActive(true);
                playerUI.SetActive(false);
                gamePaused = true;
                state = GameState.PAUSE_GAME;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
            }
            else
            {
                ResumeBtn();
            }
        }
    }

    public void Options()
    {
        if (!options)
        {
            pauseUI.SetActive(false);
            optionsUI.SetActive(true);
            options = true;
        }
        else
        {
            pauseUI.SetActive(true);
            optionsUI.SetActive(false);
            options = false;
        }
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
    }

    public void RestartGame()
    {
        sceneToLoad = "Stage1";
        StartCoroutine(LoadSceneASync());
    }

    public void PlayerDied()
    {
        pauseUI.SetActive(false);
        playerUI.SetActive(false);
        loadingUI.SetActive(false);
        wonUI.SetActive(false);
        deadUI.SetActive(true);
        state = GameState.GAME_OVER;
    }

    public void StageHander()
    {
        if (sceneToLoad == "Stage2")
        {
            pauseUI.SetActive(false);
            playerUI.SetActive(false);
            loadingUI.SetActive(false);
            deadUI.SetActive(false);
            wonUI.SetActive(false);
            continueUI.SetActive(true);
            state = GameState.PAUSE_GAME;
        }
        else if (sceneToLoad == "MainMenu")
        {
            pauseUI.SetActive(false);
            playerUI.SetActive(false);
            loadingUI.SetActive(false);
            deadUI.SetActive(false);
            wonUI.SetActive(true);
            state = GameState.GAME_OVER;
        }
    }

    public void LoadNextStage()
    {
        Debug.Log("Loading stage");
        sceneToLoad = "Stage2";
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
        state = GameState.PAUSE_GAME;
        loadingUI.SetActive(true);
        continueUI.SetActive(false);
        StartCoroutine(LoadSceneASync());
    }

    public void LoseHealth()
    {
        if (playerHealth.value >= 4)
        {
            playerUI.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = heartHalf;
            if (playerHealth.value == 4) playerUI.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = heartEmpty;
        }
        else if (playerHealth.value >= 2)
        {
            playerUI.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = heartHalf;
            if (playerHealth.value == 2) playerUI.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = heartEmpty;
        }
        else if (playerHealth.value >= 0)
        {
            playerUI.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = heartHalf;
            if (playerHealth.value == 0) playerUI. gameObject.transform.GetChild(0).GetComponent<Image>().sprite = heartEmpty;
        }
    }

    public void onHeal()
    {
        switch (playerHealth.value)
        {
            case 6:
                playerUI.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = heartFull;
                break;
            case 5:
                playerUI.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = heartHalf;
                break;
            case 4:
                playerUI.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = heartFull;
                break;
            case 3:
                playerUI.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = heartHalf;
                break;
            case 2:
                playerUI.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = heartFull;
                break;
            case 1:
                playerUI.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = heartHalf;
                break;
        }
    }

    public void SaveGameState()
    {
        PlayerState playerState = new PlayerState
        {
            weapons = GetWeaponNames(),
            health = playerHealth.value,
            currentScene = SceneManager.GetActiveScene().name
        };

        string json = JsonUtility.ToJson(playerState);
        PlayerPrefs.SetString("PlayerState", json);
        PlayerPrefs.Save();
        MainMenu();
        Debug.Log("Game state saved.");
    }

    public void LoadSavedState()
    {
        if (PlayerPrefs.HasKey("PlayerState"))
        {
            string json = PlayerPrefs.GetString("PlayerState");
            PlayerState playerState = JsonUtility.FromJson<PlayerState>(json);
            playerStateVariable.value = playerState;
            playerHealth.value = playerState.health;
            sceneToLoad = playerState.currentScene;
            gameLoadedEvent.RaiseEvent();
            Debug.Log("Game state loaded.");
        }
        else
        {
            Debug.Log("No saved game state found.");
        }
    }

    private List<string> GetWeaponNames()
    {
        List<string> weaponNames = new List<string>();
        foreach (GameObject weapon in player.GetComponent<PlayerActions>().weapons)
        {
            weaponNames.Add(weapon.name);
        }
        return weaponNames;
    }

    public void DestroyAllEnemies()
    {
        destroyAllEnemies.RaiseEvent();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
        {
            Debug.Log("Scene Loaded: " + scene.name);
            player.transform.position = GameObject.Find("SpawnPoint").transform.position;
            pauseUI.SetActive(false);
            loadingUI.SetActive(false);
            deadUI.SetActive(false);
            wonUI.SetActive(false);
            continueUI.SetActive(false);
            playerUI.SetActive(true);
            Surface2D.BuildNavMeshAsync();
            state = GameState.PLAY_GAME;
        }
    }

    private IEnumerator LoadSceneASync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncOperation.isDone)
        {
            loadingUI.GetComponentInChildren<Animator>().Play("TimingBelt");
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
