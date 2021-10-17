using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class favoriteController : MonoBehaviour
{
    public GameObject content;
    public GameObject favItem;
    public ArabicText counterController;
    public GameObject loading;
    bool finished = true;
    int counter = 0;
    string token;
    public GameObject noItems;
    private void OnEnable()
    {
        noItems.SetActive(false);
        loading.SetActive(true);
        counter = 0;
        counterController.Text = counter.ToString() + " عناصر";
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        { 
            Destroy(content.transform.GetChild(i).gameObject);
        }
      
       
        StartCoroutine(getFavorite(hostManager.domain + "api/favourites/"));
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
    IEnumerator getFavorite(string url)
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
            loading.SetActive(false);
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                StartCoroutine(getFavorite(hostManager.domain + "api/favourites/"));
           
            Debug.Log(www.error);
            }
            else

            {
                Parse(www.downloadHandler.text);
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);

                if (!string.IsNullOrEmpty(res.next))
                {
                    StartCoroutine(getFavorite(res.next));
                }
                
            }
        }
    }
    public GameObject container;
    void Parse(string response)
    {
        
        Response res = JsonUtility.FromJson<Response>(response);
        if (res.count== 0)
        {
            counter += res.results.Length;

            counterController.Text = counter.ToString() + " عناصر";
            noItems.SetActive(true);
            container.SetActive(false);
            return;
        }
        noItems.SetActive(false);
        container.SetActive(true);

        counter += res.results.Length;

        counterController.Text = counter.ToString() + " عناصر";

        for (int i = 0; i < res.results.Length; i++)
        {
            GameObject item = Instantiate(favItem);
            item.transform.parent = content.transform;
            item.GetComponent<favItemController>().createItem(res.results[i].item.name, res.results[i].item.price,res.results[i].item.images[0].file, res.results[i].item.id.ToString());

        }
    }
    public void updateCounter()
    {
        counter-- ;
        counterController.Text = counter.ToString() + " عناصر";
        if (counter == 0)
        {
            noItems.SetActive(true);
        }
    }
[System.Serializable]
public class Response
{
    public int count;
    public string next;
    public string previous;
    public resultFav[] results;
}

    [System.Serializable]
    public class resultFav
    {
        public itemFav item;
    }

    [System.Serializable]
    public class itemFav
    {
        public int id;
        public resultImages[] images;
        public string name;
        public string price;

    }

}