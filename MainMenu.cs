using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // load the game scene
        Debug.Log("playing game");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        // quit the application
        Debug.Log("Game has been quit");
        Application.Quit();
    }
}
