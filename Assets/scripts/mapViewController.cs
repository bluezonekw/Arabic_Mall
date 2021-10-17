using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class mapViewController : MonoBehaviour
{
    public GameObject clothesContent;
    public GameObject caffeContent;
    public GameObject cinemaContent;
    public GameObject mapItem;

    public List<string> mapJson=new List<string>();
    public void fillMap()
    {
        for (int i = clothesContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(clothesContent.transform.GetChild(i).gameObject);
        }
        for (int i = caffeContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(caffeContent.transform.GetChild(i).gameObject);
        }
        for (int i = cinemaContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cinemaContent.transform.GetChild(i).gameObject);
        }
        foreach(string res in mapJson)
        {
            Response response = JsonUtility.FromJson<Response>(res);
            iterateShops(response.results);
        }



    }
    private void iterateShops(resultShops[] results)
    {

        for (int i = 0; i < results.Length; i++)
        {
            addShopToMap(results[i]);
        }
    }
    private void addShopToMap(resultShops shop)
    {
        GameObject item = Instantiate(mapItem);

        if (shop.category == "Clothes")
        {
            item.transform.parent = clothesContent.transform;

        }
        else if(shop.category == "Cinema")
        {
            item.transform.parent = cinemaContent.transform;

        }
        else if(shop.category == "Caffe")
        {
            item.transform.parent = caffeContent.transform;

        }
        item.transform.GetChild(0).GetComponent<Text>().text = shop.id.ToString();
        item.transform.GetChild(1).GetComponent<ArabicText>().Text = shop.name;


        StartCoroutine(GetTextureRaw(shop.map_url, item.transform.GetChild(2).GetComponent<RawImage>()));

    }
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
        }

        /* DestroyImmediate(((DownloadHandlerTexture)www.downloadHandler).texture);
         www.Dispose();
         Resources.UnloadUnusedAssets();
         www = null;*/
    }
    public GameObject clothesScroll, cinemaScroll, caffeScroll;
    public GameObject clothesbar, cinemaBar, caffeBar;
    public void selectSection(int index)
    {
        if (index == 0)
        {
            clothesScroll.SetActive(true);
            cinemaScroll.SetActive(false);
            caffeScroll.SetActive(false);
            clothesbar.SetActive(true);
            cinemaBar.SetActive(false);
            caffeBar.SetActive(false);
        }
        else if (index == 1)
        {
            clothesScroll.SetActive(false);
            cinemaScroll.SetActive(true);
            caffeScroll.SetActive(false);
            clothesbar.SetActive(false);
            cinemaBar.SetActive(true);
            caffeBar.SetActive(false);
        }
        else if (index == 2)
        {
            clothesScroll.SetActive(false);
            cinemaScroll.SetActive(false);
            caffeScroll.SetActive(true);
            clothesbar.SetActive(false);
            cinemaBar.SetActive(false);
            caffeBar.SetActive(true);
        }
        
    }
}
