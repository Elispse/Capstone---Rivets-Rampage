using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionScreen;
    [SerializeField] private GameObject creditScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private BoolVariable gameLoaded;
 
    [SerializeField] private string sceneToLoad;

    private void Awake()
    {
        gameLoaded.value = false;
    }

    public void StartGame()
    {
        mainScreen.SetActive(false);
        optionScreen.SetActive(false);
        creditScreen.SetActive(false);
        loadingScreen.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
        sceneToLoad = "Stage1";
        StartCoroutine(LoadSceneASync());
    }

    public void MainMenu()
    {
        mainScreen.SetActive(true);
        optionScreen.SetActive(false);
        creditScreen.SetActive(false);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
    }

    public void OptionsMenu()
    {
        mainScreen.SetActive(false);
        optionScreen.SetActive(true);
        creditScreen.SetActive(false);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("PlayerState"))
        {
            mainScreen.SetActive(false);
            optionScreen.SetActive(false);
            creditScreen.SetActive(false);
            loadingScreen.SetActive(true);
            sceneToLoad = "Stage2"; // Or load the scene saved in the JSON
            gameLoaded.value = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
            StartCoroutine(LoadSceneASync());
        }
        else
        {
            Debug.Log("No saved game found.");
        }
    }

    public void CreditMenu()
    {
        mainScreen.SetActive(false);
        optionScreen.SetActive(false);
        creditScreen.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClick, this.transform.position);
        Application.Quit();
    }

    private IEnumerator LoadSceneASync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncOperation.isDone)
        {
            loadingScreen.GetComponentInChildren<Animator>().Play("TimingBelt");
            yield return null;
        }
    }
}
