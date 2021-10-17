using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;

public class gateController : MonoBehaviour
{
    public GameObject enter, exit;
    public string shopIcon;
    public GameObject welcome,header;
    IEnumerator GetTextureRaw(string url, RawImage rawImage)
    {
        Resources.UnloadUnusedAssets();
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {

            if (rawImage.texture)
            {
                Destroy(rawImage.texture);
            }

            rawImage.texture = DownloadHandlerTexture.GetContent(www);
            shoWelcome();
        }

       
    }
    void showWelcome()
    {
        welcome.GetComponent<ArabicText>().Text = " يرحب بك! ";
        StartCoroutine(GetTextureRaw(shopIcon, welcome.transform.parent.GetChild(0).gameObject.GetComponent<RawImage>()));
        
    }
    void shoWelcome()
    {
        welcome.transform.parent.gameObject.SetActive(true);
        Invoke("hideWelcome", 2f);
    }
    void hideWelcome()
    {
        welcome.transform.parent.gameObject.SetActive(false);

    }
    bool finished = true;
    string token = "";
    public void gate(GameObject Gate)
    {
        if (Gate.name == "enter")
        {
            enter.SetActive(false);
            exit.SetActive(true);
            PlayerPrefs.SetInt("EnterShopNumber", int.Parse(this.gameObject.name));
            showWelcome();
        }
        else
        {
          

            showCart(hostManager.domain + "api/cart/");
        }
       
    }
    public GameObject exitPopUp;
    public void Exit()
    {
        enter.SetActive(true);
        exit.SetActive(false);
        PlayerPrefs.SetInt("EnterShopNumber", 0);
    }
    IEnumerator getShopData()
    {
        UnityWebRequest www = UnityWebRequest.Get(hostManager.domain + "api/stores/" + this.gameObject.name);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            getShopData();
            //popup server error
            Debug.Log(www.error);
        }
        else
        {
           
            Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);

           

        }
    }
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
    // public ThirdPersonUserInput joystick;
    public void showCart(string url)
    {
        StartCoroutine(getCartItems(url));
    }

    IEnumerator getCartItems(string url)
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        using (UnityWebRequest www = UnityWebRequest.Get(hostManager.domain + "api/cart/"))
        {
           
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                showCart(hostManager.domain + "api/cart/");
                Debug.Log(www.error);
            }
            else

            {
                Response2 res = JsonUtility.FromJson<Response2>(www.downloadHandler.text);
                if (res.results.Length > 0)
                {
               //     joystick.stop();
                    header.SetActive(false);
                    exitPopUp.SetActive(true);
                    exitPopUp.GetComponent<exitPopupController>().shop = this.gameObject;
                }
                else
                {
                    Exit();
                }
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    [System.Serializable]
    public class Response2
    {
        public int id;
        public string next;
        public string previous;
        public result[] results;
    }

    [System.Serializable]
    public class result
    {
        public int id;
        public int quantity;
        public int max_quantity;
        public int item_unit;
        public string color;
        public Item item;
    }

    [System.Serializable]
    public class Item
    {
        public int id;
        public Images[] images;
        public UnitData[] units;
        public string name;
        public string description;
        public string price;

    }
    [System.Serializable]
    public class Images
    {

        public int id;
        public string file;
    }


    [System.Serializable]
    public struct UnitData
    {

        public int id;
        public string color;
        public string unit;
        public int quantity;
    }
}
