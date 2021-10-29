using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{

    public static string GamePassword { get; set; }
    public static string GameEmail { get; set; }

    [SerializeField]
    private static Text textPassword;
    [SerializeField]
    private static Text textEmail;

    public void GenerateNewData()
    {
        //GameInteger = Random.Range(1, 1000);
//GameString = System.Convert.ToBase64String(System.BitConverter.GetBytes(GameInteger));
        ShowData();
    }

    public static void ShowData()
    {
        textPassword.text = GamePassword;
        textEmail.text = GameEmail;
    }
}