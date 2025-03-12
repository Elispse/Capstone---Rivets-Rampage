using NavMeshPlus.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;

    [SerializeField] private string sceneToLoad;

    [SerializeField] private FloatVariable playerHealth;

    [SerializeField] private VoidEvent destroyAllEnemies;

    private GameState state = GameState.START_GAME;
    private RoomComplete roomComplete;
    private bool gamePaused = false;
    private bool options = false;

    private void Start()
    {
        Surface2D.BuildNavMeshAsync();
        roomComplete = generator.GetComponent<RoomComplete>();
        sceneToLoad = "MainMenu";
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
                    Time.timeScale = 1;
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

    public void PlayerWon()
    {
        pauseUI.SetActive(false);
        playerUI.SetActive(false);
        loadingUI.SetActive(false);
        deadUI.SetActive(false);
        wonUI.SetActive(true);
        state = GameState.GAME_OVER;
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
            if (playerHealth.value == 0) playerUI.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = heartEmpty;
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

    public void DestroyAllEnemies()
    {
        destroyAllEnemies.RaiseEvent();
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
}
