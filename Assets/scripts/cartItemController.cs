using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class cartItemController : MonoBehaviour
{
    public GameObject name, price;
    public Text ID,unitItem;
    public Text quantity,maxQuantity;
    public Image itemImage;

    public void setQuantity(int value)
    {
        if (((int.Parse(quantity.text)) + value > 0) && ((int.Parse(quantity.text) + value) <= (int.Parse(maxQuantity.text))))
        {
            quantity.text = ((int.Parse(quantity.text)) + value).ToString();
        }else if(((int.Parse(quantity.text)) + value <= 0))
        {
            quantity.text = ((int.Parse(quantity.text)) + value).ToString();
            delete();
            //remove from cart
        }
        transform.parent.parent.parent.parent.gameObject.GetComponent<cartController>().recalcualte();
    }

    private void Start()
    {
      
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
    string token = "";
    bool finished = true;


    public void delete()
    {
        StartCoroutine(deleteItemCart());
    }
    IEnumerator deleteItemCart()
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        string itemId = transform.GetChild(0).gameObject.GetComponent<Text>().text;
        using (UnityWebRequest www = UnityWebRequest.Delete(hostManager.domain + "api/cart/" + ID.text))
        {

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                delete();
                Debug.Log(www.error);
            }
            else
            {
                if (www.responseCode == 204)
                {
                    Debug.Log("item deleted from fav!");
                    Destroy(this.gameObject);
                    transform.parent.parent.parent.parent.gameObject.GetComponent<cartController>().recalcualte();

                }
            }
        }
    }





    public void createItem(int max_quantity, int quant, string n, string desc, string pri, string urlImage, string id,string unit_item)
    {
       
        name.gameObject.GetComponent<ArabicText>().Text = n;
        price.gameObject.GetComponent<ArabicText>().Text = pri;
        ID.text = id;
        unitItem.text = unit_item;
        quantity.gameObject.GetComponent<ArabicText>().Text = quant.ToString();
        maxQuantity.text = max_quantity.ToString();
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
}
