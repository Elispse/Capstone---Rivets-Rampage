using Edgar.Legacy.Core.LayoutGenerators.DungeonGenerator;
using Edgar.Unity;
using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private string sceneToLoad;

    private GameState state = GameState.START_GAME;
    private RoomComplete roomComplete;
    private bool gamePaused = false;

    private void Start()
    {
        Surface2D.BuildNavMeshAsync();
        roomComplete = generator.GetComponent<RoomComplete>();
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
                    state = GameState.PLAY_GAME;
                }
                break;
            case GameState.PLAY_GAME:
                
                break;
            case GameState.PAUSE_GAME:
                Time.timeScale = 0;
                break;
            case GameState.GAME_OVER:
                break;
        }
    }

    public void MainMenu()
    {
        pauseUI.SetActive(false);
        playerUI.SetActive(false);
        loadingUI.SetActive(true);
        StartCoroutine(LoadSceneASync());
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            pauseUI.SetActive(true);
            playerUI.SetActive(false);
            gamePaused = true;
            state = GameState.PAUSE_GAME;
        }
        else
        {
            pauseUI.SetActive(false);
            playerUI.SetActive(true);
            Time.timeScale = 1;
            gamePaused = false;
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
}
