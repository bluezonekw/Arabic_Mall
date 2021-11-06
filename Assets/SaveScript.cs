using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveScript : MonoBehaviour
{
    public static string GamePassword { get; set; }
    public static string GameEmail { get; set; }

    public static string savePath, textView;
    public Text Pathtext;

    void Start()
    {
        savePath = Application.persistentDataPath+ Path.DirectorySeparatorChar + "savedGames.sx";
    }
    private void Update()
    {
        Pathtext.text = textView;
    }
    public static void SaveData()
    {
        savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "savedGames.sx";

        Save save = new Save()
        {
            SavedEmail = GameEmail,
            SavedPassword = GamePassword
        };
        DeleteSave();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream;
        try
        {
            fileStream = File.Create(savePath);
            bf.Serialize(fileStream, save);
            fileStream.Close();
            Debug.Log("Data Saved");
            FileInfo fileInfo = new FileInfo(fileStream.Name);
            print( "Data Saved in : (       " + savePath + "       )  " );
        }
        catch
        {
            print("excreate");
        }






    }


    public static void DeleteSave()
    {
        savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "savedGames.sx";

        if (File.Exists(savePath))
        {
            //SaveLoad.savedGames.Clear();
            File.Delete(savePath);
            GameEmail = "";
            GamePassword = "";
            print( "Data delete in : (       " + savePath + "       )  ");

        }
        else
        {
            print("notdelete");

        }
    }

    public static void LoadData()
    {
        savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "savedGames.sx";

        if (File.Exists(savePath))
        {
            Save save;

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                FileStream fileStream = File.Open(savePath, FileMode.Open);
                save = (Save)bf.Deserialize(fileStream);

                fileStream.Close();
                GamePassword = save.SavedPassword;
                GameEmail = save.SavedEmail;

                print( "Data loaded from : (       " + savePath + "       )  ");

            }
            catch
            {
                print("exOpen");

            }



            Debug.Log("Data Loaded");
        }
        else
        {
            Debug.LogWarning("Save file doesn't exist.");
        }
    }
}