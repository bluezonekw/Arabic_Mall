using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogOutConttroller : MonoBehaviour
{
    public GameObject loading;
   public void logout()
    {
        PlayerPrefs.SetInt("MallIsCompleted", 0);
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        loading.SetActive(true);
        loadSceneWithName("UI");
    }
    public void loadSceneWithName(string scenename)
    {

        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
        Debug.Log("sceneName to load: " + scenename);
    }
}
