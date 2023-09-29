using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    None,
    Playing,
    Building,
    WorldEditing,
    Creating,
}
public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public bool IsPausing { get; private set; }
    [field: SerializeField] public GameState gameState { get; set; }

    public void ChangeGameStateButton(string state)
    {
        if (Enum.TryParse(state, out GameState newState))
        {
            gameState = newState;
        }
        else
        {
            Debug.LogWarning("Invalid game state string: " + state);
        }
    }
}
