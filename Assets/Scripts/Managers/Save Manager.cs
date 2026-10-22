using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public GameData Data;
    private int currentSaveIdx;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one SaveManager in scene");
            return;
        }
        Instance = this;
        FileHandler.SaveIndex = currentSaveIdx;

        LoadGame();
    }

    public void NewGame()
    {
        Data = new();
    }

    public bool LoadGame()
    {
        GameData m_loadedData = FileHandler.LoadFile();
        if (m_loadedData != null)
        {
            Data = FileHandler.LoadFile();
            return true;
        }

        NewGame();
        return false;
    }

    public void SaveGame()
    {
        FileHandler.SaveFile(Data);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}

public class GameData
{
}
