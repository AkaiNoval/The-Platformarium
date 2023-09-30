using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;
    public void PauseGameButton()
    {
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    public void ResumeGameButton()
    {
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }
}
