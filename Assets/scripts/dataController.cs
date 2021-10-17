
namespace Firebase.Sample.Auth
{
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.IO;
    using Firebase.Auth;
    using Firebase.Storage;
    public class dataController : MonoBehaviour
    {
        public GameObject popup;
        public ArabicText name;
        public RawImage photo;
        public InputField address;
        public InputField email;
        public InputField phoneNumber;
        bool finished = true;
        string token = "";
        Firebase.Auth.FirebaseAuth auth;
        string urlImage="";
        public void getURL()
        {
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
            // Create a root reference
            StorageReference storageRef = storage.RootReference;

            var starsRef = storageRef.Child("user_profile/" + PlayerPrefs.GetString("MallEmail") + ".jpg");

            print("user_profile/" + PlayerPrefs.GetString("MallEmail") + ".jpg");
                StorageReference pathReference =
                storage.GetReference("user_profile/" + PlayerPrefs.GetString("MallEmail") + ".jpg");
            //            storage.GetReferenceFromUrl("gs://virtual-mall-cf6d8.appspot.com");
            print(pathReference.Path);
            pathReference.GetDownloadUrlAsync().ContinueWith((Task<System.Uri> task) => {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    urlImage= task.Result.ToString(); ;
                    imageFlag = true;
                    Debug.Log("Download URL: " + task.Result);
                    // ... now download the file via WWW or UnityWebRequest.
                }
                else
                {
                    Debug.Log("Download URL Error: " + task.Result);
                    
                }
            });
        }
        void uploadImage(string path,byte[] bytes)
        {
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
            // Create a root reference
            StorageReference storageRef = storage.RootReference;
            
            // Create a reference to the file you want to upload
            StorageReference riversRef = storageRef.Child("user_profile/" + PlayerPrefs.GetString("MallEmail")+".jpg");
            Debug.Log(riversRef.Name);
            // Upload the file to the path "images/rivers.jpg"
            riversRef.PutBytesAsync(bytes)
                .ContinueWith((Task<StorageMetadata> task) => {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        Debug.Log(task.Exception.ToString());
            // Uh-oh, an error occurred!
        }
                    else
                    {
            // Metadata contains file metadata such as size, content-type, and download URL.
            StorageMetadata metadata = task.Result;
                        Debug.Log("metadata Path = " + metadata.Path);
                        //"https://randomuser.me/api/portraits/men/95.jpg"
                        updateIamgeFirebaseUser("" + metadata.Path);
                            string md5Hash = metadata.Md5Hash;
                        Debug.Log("Finished uploading...");
                        Debug.Log("md5 hash = " + md5Hash);
                    }
                });
            
        }

        void updateIamgeFirebaseUser(string photoURL)
        {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.FirebaseUser user = auth.CurrentUser;

            if (user != null)
            {
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    PhotoUrl = new System.Uri(photoURL)
                };
                user.UpdateUserProfileAsync(profile).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }
                    user = auth.CurrentUser;
                    Debug.Log(user.DisplayName);
                    Debug.Log(user.PhotoUrl);

                    Debug.Log("User profile updated successfully.");


                });
            }
        }
        IEnumerator GetTextureRaw(string url, RawImage rawImage)
        {
            print(url);
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
                rawImage.rectTransform.localEulerAngles=new Vector3(0, 0, 0);
                rawImage.texture = DownloadHandlerTexture.GetContent(www);
                // DestroyImmediate(((DownloadHandlerTexture)www.downloadHandler).texture);
            }

            
        }
        private void Start()
        {
            auth=Firebase.Auth.FirebaseAuth.DefaultInstance;
        }
        private void OnEnable()
        {
           
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
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

                    // Send token to your backend via HTTPS
                    // ...
                });
            }
            print(PlayerPrefs.GetString("MallFullName"));
            print(PlayerPrefs.GetString("MallAddress"));
            print(PlayerPrefs.GetString("MallPhoto"));
            print(PlayerPrefs.GetString("MallPhoneNumber"));
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MallFullName")))
            {
                name.Text = PlayerPrefs.GetString("MallFullName");
            }
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MallAddress")))
            {
                address.text = PlayerPrefs.GetString("MallAddress");
            }
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MallPhoneNumber")))
            {
                phoneNumber.text = PlayerPrefs.GetString("MallPhoneNumber");
            }
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MallEmail")))
            {
                email.text = PlayerPrefs.GetString("MallEmail");
            }

            getURL();
               
            

        }
        public void selectImage()
        {
            PickImage(2048);
        }
        FileStream x;
        private void PickImage(int maxSize)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                     
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize,false);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }
                    photo.texture = texture;
                    photo.rectTransform.localEulerAngles = new Vector3(0, 0, 0);

                    byte[] bytes = texture.EncodeToJPG();
                    Debug.Log(texture.EncodeToJPG());
                    uploadImage(path, texture.EncodeToJPG());

                   

                    // If a procedural texture is not destroyed manually, 
                    // it will only be freed after a scene change
                }
                Debug.Log("load texture from " + path);
            }, "Select an image", "image/");

            Debug.Log("Permission result: " + permission);
        }
        public void UpdateUser()
        {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Firebase.Auth.FirebaseUser user = auth.CurrentUser;



            if (user != null)
            {
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    DisplayName = PlayerPrefs.GetString("MallFullName"),
                    PhotoUrl = new System.Uri("https://randomuser.me/api/portraits/men/95.jpg")
                };
                user.UpdateUserProfileAsync(profile).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }
                    Debug.Log(user.DisplayName);
                    Debug.Log(user.PhotoUrl);

                    Debug.Log("User profile updated successfully.");

                });
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


        public bool IsValid(string emailaddress)
        {
            if (!string.IsNullOrEmpty(emailaddress))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(emailaddress);
                if (match.Success)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public void editPhone(InputField phone)
        {
            if (checkNumber(phone.text))
            {
                update(2, phone.text);
            }
            else
            {
                popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "يجب أن تكون صيغة رقم الهاتف لا تقل عن 8 أرقام";
                popup.SetActive(true);
            }
        }
        public void editAddress(InputField address)
        {
            
                update(1, address.text);
            
        }
        public void editEmail(InputField email)
        {
            if (IsValid(email.text))
            {
                
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Firebase.Auth.FirebaseUser user = auth.CurrentUser;
                user.UpdateEmailAsync(email.text).ContinueWith(
                task =>
                {
                    if (task.IsCanceled)
                    {
                         Debug.LogError("UpdateEmail was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        
                        Debug.LogError("UpdateEmail encountered an error: " + task.Exception);

                        string response = task.Exception.ToString();
                        if(response.IndexOf("The email address is already in use") != -1)
                        {
                            taskIndex = 1;
                        }
                        else
                        {
                            taskIndex = 2;
                        }
                        loginFlag = true;
                        return;
                    }
                    PlayerPrefs.SetString("MallEmail", email.text);


                });

                //myData = "{\"email\": \"" + value + "\"}";

                //change in firebase
            }
            else
            {
                popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "يجب أن تكون صيغة الإيميل email@example.com";
                popup.SetActive(true);
            }
        }
        int taskIndex = 0;
        public GameObject login;
        IEnumerator showLogin()
        {
            login.SetActive(true);
            yield return new WaitForSeconds(0.0f);

        }

        bool loginFlag = false;


        bool imageFlag=false;
        private void Update()
        {
            if (imageFlag)
            {
                StartCoroutine(GetTextureRaw(urlImage, photo));
                
                imageFlag = false;
            }
            if (loginFlag&& taskIndex==2)
            {
                StartCoroutine(showLogin());
                loginFlag = false;
            }else if (loginFlag && taskIndex == 1)
            {
                
                popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "هذا الإيميل مستخدم بحساب أخر";
                popup.SetActive(true);

                loginFlag = false;
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
        int tempindex;
        string tempvalue;
       
        public void update(int index, string value)
        {
            tempindex = index;
            tempvalue = value;
            StartCoroutine(updateProfile(index, value));
        }
        public GameObject loading;
        IEnumerator updateProfile(int index, string value)
        {
            loading.SetActive(true);

            while (!finished)
            {
                yield return new WaitForSeconds(0.1f);

            }
            if (token != "")
            {
                PlayerPrefs.SetString("MallTokenId", token);
            }
            string myData = "";
            if (index == 1)
            {
                myData = "{\"address\": \"" + value + "\"}";
            }
            else if (index == 2)
            {

                    myData = "{\"phone_number\": \"" + value + "\"}";
                }

               
                //  using (UnityWebRequest www = UnityWebRequest.Put(hostManager.domain + "api/profile/", myData))
                print(myData);
                using (UnityWebRequest www = UnityWebRequest.Put(hostManager.domain + "api/profile/", System.Text.Encoding.UTF8.GetBytes(myData)))
                {

                    www.SetRequestHeader("Content-Type", "application/json");
                    www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                         if (www.responseCode == 401)
                            {
                              generateToken();
                              update(tempindex, tempvalue);
                         }
                         else
                         {
                        Debug.Log(www.error);
                        Debug.Log(www.downloadHandler.text);
                        if (www.downloadHandler.text== "{\"phone_number\":[\"النموذج profile والحقل phone number موجود مسبقاً.\"]}")
                        {
                            popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "هذا الرقم مسجل من قبل يرجى اختيار رقم هاتف أخر ";
                            popup.SetActive(true);
                            loading.SetActive(false);
                        }
                         }


                        }
                    else
                    {
                        Message Msg = JsonUtility.FromJson<Message>(www.downloadHandler.text);

                        PlayerPrefs.SetString("MallFullName", Msg.full_name);
                        PlayerPrefs.SetString("MallAddress", Msg.address);
                        PlayerPrefs.SetString("MallPhoneNumber", Msg.phone_number);

                    loading.SetActive(false);
                    Debug.Log(www.downloadHandler.text);
                        Debug.Log("updated profile!");
                    }
                }

            }
        }
    class Message
    {
        public string full_name;
        public string address = null;
        public string phone_number;
        public string city;
        public bool is_complete;

    }
}