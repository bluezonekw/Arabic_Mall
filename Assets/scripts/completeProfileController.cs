using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
    public static class hostManager
{
#if UNITY_EDITOR
    public static string domain = "http://35.225.23.79/";
#elif UNITY_ANDROID 
    public static string domain = "https://mymall-kw.com/";
#elif UNITY_IOS 
    public static string domain = "https://mymall-kw.com/";
#endif
}
public class completeProfileController : MonoBehaviour
    {
    public void backfromComplete()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        gameObject.SetActive(false);
    }
        public InputField userName, Mobile;
        public Dropdown city;
    bool finished = false;
    string token = "";
    private void OnEnable()
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
    }
    public void update(Toggle privacy)
    {
        loading.SetActive(true);
        StartCoroutine(updateProfile(privacy));
        }
        IEnumerator updateProfile(Toggle privacy)
    {
        if (!finished)
        {
            yield return new WaitForSeconds(1.0f);

        }
        if (!string.IsNullOrEmpty(userName.text) && !string.IsNullOrEmpty(Mobile.text) && privacy.isOn)
        {

            if (!checkNumber(Mobile.text))
            {
               
                    popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "يجب أن تكون صيغة رقم الهاتف لا تقل عن 8 أرقام";
                popup.SetActive(true);
                loading.SetActive(false);


            }
            else
            {
                string myData = "{\"city\": \"" + (city.value + 1) + "\"," +
                    "\"full_name\":\"" + userName.text + "\"," +
                    "\"phone_number\":\"" + Mobile.text + "\"}";
                //  using (UnityWebRequest www = UnityWebRequest.Put(hostManager.domain + "api/profile/", myData))
                print(myData);
                using (UnityWebRequest www = UnityWebRequest.Put(hostManager.domain + "api/profile/", System.Text.Encoding.UTF8.GetBytes(myData)))
                {

                    www.SetRequestHeader("Content-Type", "application/json");
                    www.SetRequestHeader("Authorization", token);
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        Debug.Log(www.downloadHandler.text);

                        Message Msg = JsonUtility.FromJson<Message>(www.downloadHandler.text);
                        Debug.Log(www.downloadHandler.text);
                      
                       

                        popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "عذرا\nرقم الهاتف مسجل من قبل\nيرجى اختيار رقم هاتف أخر";
                        popup.SetActive(true);
                        loading.SetActive(false);
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
                    }
                    else
                    {
                        Message Msg = JsonUtility.FromJson<Message>(www.downloadHandler.text);

                        PlayerPrefs.SetString("MallFullName", Msg.full_name);
                          PlayerPrefs.SetString("MallAddress", Msg.address);
                          PlayerPrefs.SetString("MallPhoneNumber", Msg.phone_number);
                          
                        Debug.Log(www.downloadHandler.text);
                        Debug.Log("updated profile!");
                        loadSceneWithName("mall");
                    }
                }
            }
        }
        else
        {
            loading.SetActive(false);
            if (!privacy.isOn)
            {
                popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "يجب الموافقة علي الشروط والاحكام";
            }
            else
            {
                popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "الرجاء إكمال البيانات\nلاتمام عملية التسجيل\nبنجاح";

            }
            popup.SetActive(true);

        }
    }
    bool checkNumber(string number)
    {
        if (number.Length == 8)
        {
            return true;
        }
        return false;
    }
    class Message
    {
        public string full_name;
        public string address = null;
        public string phone_number;
        public string city;
        public bool is_complete;

    }
    public GameObject popup,loading;
        public void loadSceneWithName(string scenename)
        {
        loading.SetActive(false);
            Debug.Log("sceneName to load: " + scenename);
            SceneManager.LoadScene(scenename, LoadSceneMode.Single);
            Debug.Log("sceneName to load: " + scenename);
        }
    }
