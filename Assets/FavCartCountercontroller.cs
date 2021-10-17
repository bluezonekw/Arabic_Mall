using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FavCartCountercontroller : MonoBehaviour
{
    public GameObject favCounter;
    public GameObject cartCounter;
    string token="";
    bool finished = true;
    public void generateToken()
    {
        finished = false;

        Firebase.Auth.FirebaseAuth auth;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;

        user.TokenAsync(true).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }
            finished = true;
            string idToken = task.Result;
            token = idToken;

           

        });

    }
    public void reCall()
    {
        showCounter(hostManager.domain + "api/user/items/count/");

    }
    void OnEnable()
    {

      

        showCounter(hostManager.domain + "api/user/items/count/");

    }

    public void showCounter(string url)
    {
        generateToken();
        StartCoroutine(getCounterValue(url));
    }

    IEnumerator getCounterValue(string url)
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                showCounter(hostManager.domain + "api/user/items/count/");
                Debug.Log(www.error);
            }
            else

            {

                print(www.downloadHandler.text);
                    ResponseCounter res = JsonUtility.FromJson<ResponseCounter>(www.downloadHandler.text);
                if (res.favourites_count > 0)
                {
                    favCounter.SetActive(true);
                    favCounter.transform.GetChild(0).gameObject.GetComponent<Text>().text = res.favourites_count.ToString();
                }
                else
                {
                    favCounter.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";
                    favCounter.SetActive(false) ;

                }
                
                if (res.cart_items_count > 0)
                {
                    cartCounter.SetActive(true);
                    cartCounter.transform.GetChild(0).gameObject.GetComponent<Text>().text = res.cart_items_count.ToString();
                }
                else
                {
                    cartCounter.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";

                    cartCounter.SetActive(false); 
                   
                }


            }
        }

    }
    [System.Serializable]
    public class ResponseCounter
    {
        public int favourites_count;
        public int cart_items_count;
    }
}
