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
        if (!File.Exists(SaveLocation))
        {
            return null;
        }

        string m_loadedData;

        using (FileStream s = new(SaveLocation, FileMode.Open))
        {
            using (StreamReader r = new(s))
            {
                m_loadedData = r.ReadToEnd();
            }
        }
        m_loadedData = EncoderDecoder(EncryptionKey, m_loadedData);
        return JsonUtility.FromJson<GameData>(m_loadedData);
    }

    public static bool SaveFile(GameData data)
    {
        try
        {
            string m_jsonData = JsonUtility.ToJson(data);
            m_jsonData = EncoderDecoder(EncryptionKey, m_jsonData);

            using (FileStream s = new(SaveLocation, FileMode.OpenOrCreate))
            {
                using (StreamWriter w = new(s))
                {
                    w.Write(m_jsonData);
                    return true;
                }
            }
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    private static string EncoderDecoder(string key, string data)
    {
        string m_translatedString = "";
        for (int i = 0; i < data.Length; i++)
        {
            m_translatedString += (char)(data[i] ^ key[i % key.Length]);
        }
        return m_translatedString;
    }
}