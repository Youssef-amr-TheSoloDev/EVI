using System;
using UnityEngine;

public class GameEventBus : MonoBehaviour
{
    public static GameEventBus Instance { get; private set; }

    public Action<GameObject> OnPlanetHaver, OnPlanetSelect;
    public Action OnInputToggle;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one event bus in scene");
            return;
        }
        Instance = this;
    }
}
