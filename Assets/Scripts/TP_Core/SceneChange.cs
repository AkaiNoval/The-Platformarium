using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeToGameplayScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ChangeToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Application.Quit(); // Exit the application
    }
}
