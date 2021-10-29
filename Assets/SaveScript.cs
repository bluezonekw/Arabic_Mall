using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class SaveScript : MonoBehaviour
{

    private static string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/gamesave.save";
    }

    public static void SaveData()
    {
        var save = new Save()
        {
            SavedEmail = GameData.GameEmail,
            SavedPassword = GameData.GamePassword
        };
        DeleteSave();
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }

        Debug.Log("Data Saved");
    }


    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            //SaveLoad.savedGames.Clear();

            File.Delete(savePath);

        }
    }

    public static void LoadData()
    {
        if (File.Exists(savePath))
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            GameData.GamePassword = save.SavedPassword;
            GameData.GameEmail = save.SavedEmail;
            GameData.ShowData();

            Debug.Log("Data Loaded");
        }
        else
        {
            Debug.LogWarning("Save file doesn't exist.");
        }
    }
}