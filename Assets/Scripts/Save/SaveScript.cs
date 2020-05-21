using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveScript
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.o";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        formater.Serialize(fileStream, data);
        fileStream.Close();
    }
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.o";
        if (File.Exists(path))
        {
            BinaryFormatter formater = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            PlayerData playerData = formater.Deserialize(fileStream) as PlayerData;
            fileStream.Close();
            return playerData;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
    public static void SaveLevel(Level level)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level.o";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(level);
        formater.Serialize(fileStream, data);
        fileStream.Close();
    }
    public static LevelData LoadLevel()
    {
        string path = Application.persistentDataPath + "/level.o";
        if (File.Exists(path))
        {
            BinaryFormatter formater = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            LevelData playerData = formater.Deserialize(fileStream) as LevelData;
            fileStream.Close();
            return playerData;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
}
