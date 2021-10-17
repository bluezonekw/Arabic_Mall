using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class selectedItemController : MonoBehaviour
{
    Dictionary<string, List<string>> dic;
    public Sprite fav, unFav;
    public Text quantity, description, name, price,ID;
    public Image favIcon, itemIcon;
    bool finished = true, is_fav = false;
    string token;
    public ArabicText units, colors;

    public GameObject items, favorites,cart,loading;
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
    public void backfromItem()
    {
        GameObject counter = GameObject.FindGameObjectWithTag("carFavControllerCounter");
        counter.GetComponent<FavCartCountercontroller>().reCall();

        if (PlayerPrefs.GetInt("directPurchase") == 2)
        {
            favorites.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("directPurchase") == 1)
        {
            cart.SetActive(true);
        }
        else
        {
           items.SetActive(true);
            items.GetComponent<itemController>().showItems(PlayerPrefs.GetString("EnterStandNumber"));

        }
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {

      //  loading.SetActive(true);
        dic =new Dictionary<string, List<string>>();
      
        quantityValue.text = "0";
        
    }
   
    List<string> getKeys(Dictionary<string,List<string>> dic)
    {
        List<string> keys = new List<string>();
        foreach (KeyValuePair<string, List<string>> kvp in dic)
        {
            keys.Add(kvp.Key);
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value.Count));
        }
        return keys;

    }
    void fillDictionary(UnitData[] units)
    {
        foreach( UnitData x in units)
        {
            List<string> aa = new List<string>();
            aa.Add(x.color);
            if (!dic.ContainsKey(x.unit))
            {
                dic.Add(x.unit, new List<string>(aa));
            }
            else
            {
                dic[x.unit].Add(x.color);
            }
        }

        foreach (KeyValuePair<string, List<string>> kvp in dic)
        {
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Debug.Log( string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value.Count));
        }
    }
    public GameObject popupSize, popupColor, popupSizeParent, popupColorParent, selectedSize, selectedColor;
    void PopulatePopUpSize(List<string> optionsArray)
    {
        for (int i = popupSize.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(popupSize.transform.GetChild(i).gameObject);
        }
        //create our own popup selection
        print(optionsArray.Count);
        foreach (var option in optionsArray)
        {

            print(option);
            GameObject item = Instantiate(selectedSize);
            item.transform.GetChild(1).gameObject.GetComponent<ArabicText>().Text = option;
            item.transform.parent = popupSize.transform;

        }

    }
    void PopulatePopUpColor(List<string> optionsArray)
    {
        for (int i = popupColor.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(popupColor.transform.GetChild(i).gameObject);
        }
        //create our own popup selection
        print(optionsArray.Count);
        foreach (var option in optionsArray)
        {

            print(option);
            GameObject item = Instantiate(selectedColor);
            item.transform.GetChild(1).gameObject.GetComponent<ArabicText>().Text = option;
            item.transform.parent = popupColor.transform;

        }

    }
    void PopulateDropdown(Dropdown dropdown, List<string> optionsArray)
    {

        //create our own popup selection
        print(optionsArray.Count);
        List<string> options = new List<string>();
        foreach (var option in optionsArray)
        {
            print(option);
            options.Add(option); // Or whatever you want for a label
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
    string itemID = "";
    public void getData(string id)
    {
        itemID = id;
        print(" ID : " + id);
        StartCoroutine(getItemData(id));

    }
    item it;
    IEnumerator getItemData(string id) {
        ID.text = id;
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        using (UnityWebRequest www = UnityWebRequest.Get(hostManager.domain + "api/items/" + id))
        {

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            print(token);
            yield return www.SendWebRequest();
           //////////////// loading.SetActive(false);
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                getData(itemID);
                Debug.Log(www.error);
            }
            else
            {
                print(www.downloadHandler.text);
                it = JsonUtility.FromJson<item>(www.downloadHandler.text);
                //string x = "{\"id\":2,\"images\":[{\"id\":4,\"file\":\"http://35.225.23.79/media/131966596_1642135185969969_8226673099786467013_n.jpg\"}],\"is_favourite\":true,\"units\":[{\"id\":6,\"unit\":\"مقاس 32\",\"quantity\":2,\"color\":\"أحمر\"},{\"id\":6,\"unit\":\"مقاس 32\",\"quantity\":2,\"color\":\"شش\"},{\"id\":7,\"unit\":\"مقاس 33\",\"quantity\":1,\"color\":\"أحمر\"},{\"id\":8,\"unit\":\"مقاس 34\",\"quantity\":2,\"color\":\"no-color\"}],\"name\":\"Am\",\"description\":\"ةا\",\"price\":\"66.000\",\"stand\":1}";
                //item it = JsonUtility.FromJson<item>(x);
                print(it.units.Length);
                fillDictionary(it.units);
                is_fav = it.is_favourite;
                if (is_fav)
                {
                    favIcon.sprite = fav;
                }
                else
                {
                    favIcon.sprite = unFav;
                }

                name.gameObject.GetComponent<ArabicText>().Text = it.name;
                price.gameObject.GetComponent<ArabicText>().Text = it.price;
                description.gameObject.GetComponent<ArabicText>().Text = it.description;

                string unit1 = getKeys(dic)[0];
                units.Text = unit1;

                colors.Text = dic[unit1][0];

                //is_fav

                //favIcon.sprite = unFav;
                StartCoroutine(GetTexture(it.images[0].file, itemIcon));
                

              

            }
        }
    }
    public Text quantityValue;
    public void setQuantity(int value)
    {
        if(((int.Parse(quantityValue.text))+ value >= 0)  && ((int.Parse(quantityValue.text) + value) <= getQuantity(units.Text, colors.Text))){
            quantityValue.text = ((int.Parse(quantityValue.text)) + value).ToString();
        }
    }
    int getQuantity(string key, string value)
    {
        foreach (UnitData x in it.units)
        {
            if (x.unit == key && x.color == value)
            {
                return x.quantity;
            }
        }
        return 0;
    }

    int getUnitID(string key, string value)
    {
        foreach (UnitData x in it.units)
        {
            if (x.unit == key && x.color == value)
            {
                return x.id;
            }
        }
        return -1;
    }
    public void openPopupSize()
    {
        PopulatePopUpSize(getKeys(dic));
        popupSizeParent.SetActive(true);
    }
    
    public void setSize(string value)
    {
        units.Text = value;
        colors.Text = dic[value][0];
        popupSizeParent.SetActive(false);
    }
    public void openPopupColor()
    {
        PopulatePopUpColor(dic[units.Text]);
        popupColorParent.SetActive(true);
    }

    public void setColor(string value)
    {
        colors.Text = value;
        popupColorParent.SetActive(false);
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
        print(is_fav);
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

                    is_fav = false;
                    favIcon.sprite = unFav;
                    GameObject counter = GameObject.FindGameObjectWithTag("carFavControllerCounter");
                    counter.GetComponent<FavCartCountercontroller>().reCall();
                }
            }
        }
    }



   
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
                    favIcon.sprite = fav;
                }
            }
        }


    }














    public void addItemToCart()
    {

        if(int.Parse(quantityValue.text) > 0&& getUnitID(units.Text, colors.Text) > 0)
        {
            StartCoroutine(addtoCart(getUnitID(units.Text, colors.Text), int.Parse(quantityValue.text)));
        }



       
    }

    IEnumerator addtoCart(int unitItem,int unitQuantity)
    {

        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        WWWForm form = new WWWForm();
        form.AddField("item_unit", unitItem);
        form.AddField("quantity", unitQuantity);

        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(hostManager.domain + "api/cart/", form))
        {

            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                addItemToCart();
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
                    backfromItem();
                    print("added to cart");
                }
            }
        }
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
        public UnitData[] units;

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
