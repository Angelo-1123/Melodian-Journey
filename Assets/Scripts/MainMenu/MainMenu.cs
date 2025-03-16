using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controls;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenuToControls()
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
