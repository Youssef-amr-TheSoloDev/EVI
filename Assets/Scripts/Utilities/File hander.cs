using UnityEngine;
using System.IO;

public static class FileHandler
{
    private static string SaveLocation = Application.persistentDataPath;
    private const string EncryptionKey = "81hzm65l";
    public static int SaveIndex
    {
        get => SaveIndex;
        set
        {
            SaveLocation = Application.persistentDataPath + $"Save{value}";
            SaveIndex = value;
        }
    }

    public static GameData LoadFile()
    {
        return new();
    }

    public static bool SaveFile(GameData data)
    {
        return false;
    }

    private static string EncoderDecoder()
    {
        return "";
    }
}