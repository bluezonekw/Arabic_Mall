using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class itemController : MonoBehaviour
{
    public GameObject content;
    public GameObject back;

    public GameObject standItem;
    public GameObject loading;

    string token = "";
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
    private void OnEnable()
    {
       // loading.SetActive(true);
       
        
    }
    string standID;
    public void showItems(string standNumber)
    {
        standID = standNumber;
        PlayerPrefs.SetString("EnterStandNumber", standNumber);
        PlayerPrefs.SetString("EnterStandNumber", standNumber);
        StartCoroutine(getItems(standNumber));
            }

    IEnumerator getItems(string standNumber)
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        using (UnityWebRequest www = UnityWebRequest.Get(hostManager.domain + "api/stores/"+ PlayerPrefs.GetInt("EnterShopNumber")+"/stands/"+ standNumber))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
           // loading.SetActive(false);

            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                StartCoroutine(getItems(standID));
                Debug.Log(www.error);
            }
            else

            {
                Debug.Log(www.downloadHandler.text);
                Parse(www.downloadHandler.text);
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    public GameObject noItems;
    void Parse(string response)
    {
        noItems.SetActive(false);
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        Response res= JsonUtility.FromJson<Response>(response);
        if (res.items.Length == 0)
        {
            noItems.SetActive(true);

        }
        for (int i = 0; i < res.items.Length; i++)
        {
            GameObject item = Instantiate(standItem);
            item.transform.parent = content.transform;
            item.GetComponent<itemManager>().createItem(res.items[i].is_favourite, res.items[i].name, res.items[i].description, res.items[i].price, res.items[i].images[0].file, res.items[i].id.ToString());

        }

        back.SetActive(true);
    }



















    [System.Serializable]
    public class Response
    {
        public int id;
        public item[] items;
    }

    [System.Serializable]
    public class item
    {
        public int id;
        public bool is_favourite;
        public string name;
        public string description;
        public string price;
        public Images[] images;

    }
    [System.Serializable]
    public class Images
    {

        public int id;
        public string file;
    }
    
}
