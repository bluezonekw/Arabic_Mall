using RestSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogOutConttroller : MonoBehaviour
{
    public GameObject loading;
   public void logout()
    {

        SaveScript.DeleteSave();

        loading.SetActive(true);

        var client = new RestClient("https://mall.openshoop.com/api/V1/logout");
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        request.AddHeader("password_api", "mall_2021_m3m");
        request.AddHeader("lang_api", "ar");

        try
        {
            request.AddHeader("auth-token",ApiClasses.Register.data.token);
            IRestResponse response = client.Execute(request);
        }
        catch
        {
            request.AddHeader("auth-token", ApiClasses.Login.data.original.access_token);
            IRestResponse response = client.Execute(request);
        }
        loadSceneWithName("UI");
    }
    public void loadSceneWithName(string scenename)
    {

        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
        Debug.Log("sceneName to load: " + scenename);
    }
}
