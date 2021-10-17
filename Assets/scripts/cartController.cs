using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class cartController : MonoBehaviour
{
    public GameObject content;

    public GameObject cartItem;
    string token = "";
    bool finished = true;
    public ArabicText totalPrice;
    public ArabicText counterController;
    int counter = 0;
    public GameObject loading;
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

    // Start is called before the first frame update
    void OnEnable()
    {
        noItems.SetActive(false); 
        loading.SetActive(true);
           counter = 0;
        counterController.Text = counter.ToString() + " عناصر";

        totalPrice.Text = "0 جنيه";
       
        
        
        showCart(hostManager.domain + "api/cart/");
    }
    public GameObject continueShopping;

    public bool fromExit = false;
    public GameObject exitPopUp;
    public GameObject header;
    public void saveTotal()
    {
        string total = totalPrice.Text.Replace(" جنيه", "");
        print(total);
        PlayerPrefs.SetString("MallCost", total);
    }
    public void back()
    {
        if (fromExit)
        {
            exitPopUp.SetActive(true);
        }
        else
        {

            header.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }
    public void showCart(string url)
    {
        StartCoroutine(getCartItems(url));
    }

    IEnumerator getCartItems(string url)
    {
        print(finished);
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
                showCart(hostManager.domain + "api/cart/");
                Debug.Log(www.error);
            }
            else

            {
                Debug.Log(www.downloadHandler.text);
            
            Parse(www.downloadHandler.text);
                if (fromExit)
                {
                    continueShopping.SetActive(false);
                }
                else
                {

                    continueShopping.SetActive(true);
                }            }
        }
    }
    public GameObject noItems;
    public GameObject container;
    void Parse(string response)
    {
        print(response);
        float total = 0.0f;
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        Response res = JsonUtility.FromJson<Response>(response);
        if (res.count == 0)
        {

            counter += res.results.Length;

            counterController.Text = counter.ToString() + " عناصر";

            noItems.SetActive(true);
            container.SetActive(false);
            return;
        }
        noItems.SetActive(false);
        container.SetActive(true);

        if (!string.IsNullOrEmpty(res.next))
        {
            showCart(res.next);

        }
        counter += res.results.Length;

        counterController.Text = counter.ToString() + " عناصر";
        for (int i = 0; i < res.results.Length; i++)
        {
            total += ((float)res.results[i].quantity * float.Parse(res.results[i].item.price));
            GameObject item = Instantiate(cartItem);
            item.transform.parent = content.transform;
            item.GetComponent<cartItemController>().createItem(res.results[i].max_quantity, res.results[i].quantity, res.results[i].item.name, res.results[i].item.description, res.results[i].item.price, res.results[i].item.images[0].file, res.results[i].id.ToString(), res.results[i].item_unit.ToString());

        }
        PlayerPrefs.SetString("MallShipping", res.results[0].shipping_price.ToString());

        totalPrice.Text = total+ " جنيه";
    }
    
    public void recalcualte()
    {
        float total = 0.0f;
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            total += (float.Parse(content.transform.GetChild(i).GetChild(9).gameObject.GetComponent<Text>().text) * float.Parse(content.transform.GetChild(i).GetChild(5).gameObject.GetComponent<ArabicText>().Text));
            
        }
        Invoke("count", 0.3f);
       
        totalPrice.Text = total + " جنيه";
    }
    void count()
    {
        counterController.Text = content.transform.childCount + " عناصر";

        if (content.transform.childCount == 0)
        {
            noItems.SetActive(true);
        }
    }
    public void clearCart()
    {
        StartCoroutine(clearCartItems());
    }

    IEnumerator clearCartItems()
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        using (UnityWebRequest www = UnityWebRequest.Delete(hostManager.domain + "api/clear-cart/"))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                clearCart();
                Debug.Log(www.error);
            }
            else

            {
                Debug.Log(www.responseCode);
                if (www.responseCode == 204)
                {
                    if (fromExit)
                    {
                        exitPopUp.GetComponent<exitPopupController>().shop.GetComponent<gateController>().Exit();
                    }
                    fromExit = false;
                    for (int i = content.transform.childCount - 1; i >= 0; i--)
                    {
                        Destroy(content.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
    }


    [System.Serializable]
    public class Response
    {
        public int count;
        public string next;
        public string previous;
        public result[] results;
    }

    [System.Serializable]
    public class result
    {
        public int id;
        public int quantity;
        public float shipping_price;
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