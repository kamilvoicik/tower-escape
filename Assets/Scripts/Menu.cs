using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnStartFirstLevelButton()
    {
        SceneManager.LoadScene(1);
    }
    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitGameButton()
    {
        Application.Quit();
    }
}
