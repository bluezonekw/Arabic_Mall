using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class favItemController : MonoBehaviour
{
    public Text name,price,ID;
    public Image itemImage;
    public void openItem(int purchase)
    {
       
        PlayerPrefs.SetInt("directPurchase", purchase);
        this.transform.parent.gameObject.GetComponent<itemHolder>().item.SetActive(true);
        this.transform.parent.gameObject.GetComponent<itemHolder>().item.gameObject.GetComponent<selectedItemController>().getData(ID.text);
        this.transform.parent.parent.parent.parent.parent.gameObject.SetActive(false);
    }
    private void Start()
    {
        Firebase.Auth.FirebaseAuth auth;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        user.TokenAsync(true).ContinueWith(task => {
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
            // Send token to your backend via HTTPS
            // ...
        });
    }string token = "";
    bool finished = true;
    public void createItem(string n,  string pri, string urlImage, string id)
    {
       
        name.gameObject.GetComponent<ArabicText>().Text = n;
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


    public void buyItem()
    {
        //add to cart then hide favorite list and show item object
    }

    public void unfavorite()
    {
        StartCoroutine(deleteFav());
    }
    IEnumerator deleteFav()
    {
        string itemId = transform.GetChild(0).gameObject.GetComponent<Text>().text;
        using (UnityWebRequest www = UnityWebRequest.Delete(hostManager.domain + "api/unfavourite/" + itemId))
        {

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization",token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

               
                if (www.responseCode == 204)
                {
                    GameObject counter = GameObject.FindGameObjectWithTag("carFavControllerCounter");
                    counter.GetComponent<FavCartCountercontroller>().reCall();
                    Debug.Log("item deleted from fav!");
                    Destroy(this.gameObject);

                    if (transform.parent.parent.parent.gameObject.GetComponent<favoriteController>() as favoriteController != null)
                    {
                        transform.parent.parent.parent.gameObject.GetComponent<favoriteController>().updateCounter();
                    }
                    else
                    {
                        transform.parent.parent.parent.parent.gameObject.GetComponent<favoriteController>().updateCounter();

                    }
                }
                
            }
        }
    } 
}
