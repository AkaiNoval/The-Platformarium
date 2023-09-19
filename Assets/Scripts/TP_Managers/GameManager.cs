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
    [field: SerializeField] public GameState gameState { get; private set; }
}
