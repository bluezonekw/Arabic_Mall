using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using ArabicSupport;
public class plateController : MonoBehaviour
{
    public mapViewController mapController;

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
    public GameObject[] stands;
    IEnumerator GetTexture(string url, Material mat)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {

            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            mat.mainTexture = myTexture;
          }

        www.Dispose();
        Resources.UnloadUnusedAssets();

        www = null;
    }


    IEnumerator GetTextureRaw(string url, RawImage rawImage)
    {

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
           // DestroyImmediate(((DownloadHandlerTexture)www.downloadHandler).texture);
         }

        DestroyImmediate(((DownloadHandlerTexture)www.downloadHandler).texture);
        www.Dispose();
        Resources.UnloadUnusedAssets();
        www = null;
    }
    //UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://35.225.23.79/api/stores/");
    //Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    public plateData[] shops;
    public RawImage[] plate;
    public gateController[] shopsName;


    void Start()
    {
        StartCoroutine(getShops(hostManager.domain + "api/stores/"));
        checkCart();
    }
    bool finished = true;
    string token = "";
    void checkCart()
    {
        generateToken();



            showCart(hostManager.domain + "api/cart/");
        
    }

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
               
                Debug.Log(www.downloadHandler.text);
            }
        }

    }
    public GameObject exitPopUp;
    public GameObject header;


















    // Update is called once per frame
    IEnumerator getShops(string url)
    {
        print(url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            //popup server error
            Debug.Log(www.error);
        }
        else
        {

            Response response= JsonUtility.FromJson<Response>(www.downloadHandler.text);
            // updatePlates(response.results);
            // updateStands(response.results);
            mapController.mapJson.Add(www.downloadHandler.text);
            mapController.fillMap();
            if (updatePlates(response.results) && updateStands(response.results) && !string.IsNullOrEmpty( response.next ))
            {
                StartCoroutine(getShops(response.next));
            }
            
        }

       /* Resources.UnloadUnusedAssets();
        www.Dispose();
        www = null;
        */
    }
    bool updatePlates(resultShops[] results)
    {
        for (int i = 0; i < results.Length; i++)
        {
            for (int j = 0; j < results[i].images.Length; j++)
            {

                StartCoroutine(GetTexture(results[i].images[j].file, shops[results[i].id - 1].plates[j]));
               // StartCoroutine(GetTextureRaw(results[i].images[0].file, plate[results[i].id - 1]));

            }
        }
        return true;
    }
    bool updateStands(resultShops[] results)
    {
        int id;
        for (int i = 0; i < results.Length; i++)
        {

            if ((results[i].id - 1) < shopsName.Length)
            {
                shopsName[results[i].id - 1].shopIcon = results[i].map_url;
                for (int j = 0; j < results[i].stands.Length; j++)
                {
                    id = results[i].id;

                    /* if (results[i].stands[j].id.ToString() == stands[id - 1].transform.GetChild(j).gameObject.name)
                     {
                         for (int h = 0; h < 4; h++)
                         {
                          //   stands[id - 1].transform.GetChild(j).GetChild(0).GetChild(h).gameObject.GetComponent<ArabicText>().Text = results[i].stands[j].name;
                             stands[id - 1].transform.GetChild(j).GetChild(0).GetChild(h).gameObject.GetComponent<Text>().text = ArabicFixer.Fix(results[i].stands[j].name);
                         }
                     }*/
                }
            }
        }
        return true;
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
[System.Serializable]
public class Response
{
    public int count;
    public string next;
    public string previous;
    public resultShops[] results;
}

[System.Serializable]
public class resultShops
{
    public int id;
    public string name;
    public string category;
    public resultImages[] images;
    public resultStands[] stands;
    public string map_url;

}

[System.Serializable]
public class resultImages
{

    public int id;
    public string file;
}
[System.Serializable]
public class resultStands
{

    public int id;
    public string name;
}
