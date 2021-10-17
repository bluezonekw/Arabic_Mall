using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Firebase.Auth;
public class itemManager : MonoBehaviour
{
    public Sprite fav, unFav;
    public Image FAVIcon,itemImage;
    public Text name, description, price,ID;

    bool is_fav = false;
    private void Start()
    {
      
    }
    public void openItem(int purchase)
    {
        PlayerPrefs.SetInt("directPurchase",purchase);
        this.transform.parent.gameObject.GetComponent<itemHolder>().item.SetActive(true);
        this.transform.parent.gameObject.GetComponent<itemHolder>().item.gameObject.GetComponent<selectedItemController>().getData(ID.text);
        this.transform.parent.parent.parent.parent.gameObject.SetActive(false);
    }
    public void createItem(bool isFav, string n, string desc, string pri, string urlImage,string id)
    {
        is_fav = isFav;

        if (isFav)
        {
            FAVIcon.sprite =fav;
        }
        else
        {
            FAVIcon.sprite = unFav;
        }
        name.gameObject.GetComponent<ArabicText>().Text = n;
        //name.text = n;
        description.gameObject.GetComponent<ArabicText>().Text = desc;
        price.gameObject.GetComponent<ArabicText>().Text = pri;
        ID.text = id;
        StartCoroutine(GetTexture(urlImage, itemImage));

    }


    IEnumerator GetTexture(string url, Image img)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            img.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));

           
        }
    }
    public void starFunction()
    {
       
        if (is_fav)
        {
            unfavorite();
        }
        else
        {
            favorite();
        }
       
    }
    public void unfavorite()
    {
        StartCoroutine(deleteFav());
    }
    IEnumerator deleteFav()
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        using (UnityWebRequest www = UnityWebRequest.Delete(hostManager.domain + "api/unfavourite/" + ID.text))
        {

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();

                unfavorite();
                Debug.Log(www.error);
            }
            else
            {
                if (www.responseCode == 204)
                {

                    GameObject counter = GameObject.FindGameObjectWithTag("carFavControllerCounter");
                    counter.GetComponent<FavCartCountercontroller>().reCall();
                    is_fav = false;
                    FAVIcon.sprite = unFav;

                }
            }
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

    bool finished = true;

    string token = "";
    public void favorite()
    {
       
        StartCoroutine(addFav());
    }
    IEnumerator addFav()
    {

        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        // string myData = "{\"item\": " + ID.text + "}";
        //print(myData);
        WWWForm form = new WWWForm();
        form.AddField("item", ID.text);
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
      

        using (UnityWebRequest www = UnityWebRequest.Post(hostManager.domain + "api/favourites/", form))
         {

             www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
             www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
             yield return www.SendWebRequest();
             if (www.isNetworkError || www.isHttpError)
             {
                generateToken();

                favorite();
                 Debug.Log(www.error);
                 Debug.Log(www.downloadHandler.text);
             }
             else
             {
                print(www.responseCode);
                 if (www.responseCode == 201)
                 {
                    GameObject counter = GameObject.FindGameObjectWithTag("carFavControllerCounter");
                    counter.GetComponent<FavCartCountercontroller>().reCall();
                    is_fav = true;
                    FAVIcon.sprite = fav;
                 }
             }
         }

    
    }
}
