using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionScreen;
    [SerializeField] private GameObject creditScreen;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        mainScreen.SetActive(true);
        optionScreen.SetActive(false);
        creditScreen.SetActive(false);
    }

    public void OptionsMenu()
    {
        mainScreen.SetActive(false);
        optionScreen.SetActive(true);
        creditScreen.SetActive(false);
    }

    public void CreditMenu()
    {
        mainScreen.SetActive(false);
        optionScreen.SetActive(false);
        creditScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
