
namespace Firebase.Sample.Auth
{
    using Firebase.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.Networking;
    using UnityEngine.Events;
    using Firebase.Auth;
    using Facebook.Unity;
   
    public class authController : MonoBehaviour
    {
        bool popUpFlag = false;
        string msg="";
        public GameObject splash;
        private void Update()
        {
            if (popUpFlag)
            {
                loadingObject.SetActive(false);
                StartCoroutine(showPopUp(msg));
                popUpFlag = false;
            }
        }
        private Firebase.FirebaseApp app;
        public Firebase.FirebaseApp App
        {
            get
            {
                if (app == null)
                {
                    app = Firebase.FirebaseApp.DefaultInstance;
                }
                return app;
            }
        }
        private static authController instance;
        public static authController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new authController();
                }
                return instance;
            }
        }

        protected Firebase.Auth.FirebaseAuth auth;
       
        private Firebase.AppOptions otherAuthOptions = new Firebase.AppOptions
        {
            ApiKey = "",
            AppId = "",
            ProjectId = ""
        };
        protected Firebase.Auth.FirebaseAuth otherAuth;
        void InitializeFirebase()
        {
            //loadingObject.SetActive(true);
            Debug.Log("Setting up Firebase Auth");
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
           
            auth.StateChanged += AuthStateChanged;
            auth.IdTokenChanged += IdTokenChanged;
            // Specify valid options to construct a secondary authentication object.
            if (otherAuthOptions != null &&
                !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) ||
                  String.IsNullOrEmpty(otherAuthOptions.AppId) ||
                  String.IsNullOrEmpty(otherAuthOptions.ProjectId)))
            {
                try
                {
                    otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
                      otherAuthOptions, "Secondary"));
                    otherAuth.StateChanged += AuthStateChanged;
                    otherAuth.IdTokenChanged += IdTokenChanged;
                }
                catch (Exception)
                {
                    Debug.Log("ERROR: Failed to initialize secondary authentication object.");
                }
            }
            AuthStateChanged(this, null);




        }
        protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
          new Dictionary<string, Firebase.Auth.FirebaseUser>();
        void IdTokenChanged(object sender, System.EventArgs eventArgs)
        {
            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
            {
                senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
                //,
                //task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result)));
                task => PlayerPrefs.SetString("EMallUserToken", task.Result));
            }
        }
        private bool fetchingToken = false;
        void showLogin()
        {
            print("show Login");
            GameObject splash = GameObject.FindGameObjectWithTag("splash");

            splash.transform.GetChild(0).gameObject.SetActive(false) ;
            GameObject container = GameObject.FindGameObjectWithTag("login");

            container.transform.GetChild(0).gameObject.SetActive(true);
        }
        void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            Firebase.Auth.FirebaseUser user = null;
            //senderAuth.SignOut();
            if (senderAuth != null)
            {
              //  Invoke("showLogin",2f);

                print("senderAuth != null");
                userByAuth.TryGetValue(senderAuth.App.Name, out user);

            }
            else
            {
             //   Invoke("showLogin", 2f);
                print("you need to login");
                loadingObject.SetActive(false);
            }
            if (senderAuth == auth && senderAuth.CurrentUser != user)
            {

                bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
                if (!signedIn && user != null)
                {
                    Debug.Log("Signed out " + user.UserId);
                }
                user = senderAuth.CurrentUser;
                userByAuth[senderAuth.App.Name] = user;
                if (signedIn)
                {
                    
                    Firebase.Auth.FirebaseAuth auth;
                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                    user = auth.CurrentUser;

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
                    Debug.Log("AuthStateChanged Signed in " + user.UserId);
                  //  displayName = user.DisplayName ?? "";
                    DisplayDetailedUserInfo(user, 1);
                }
                else
                {
                    Invoke("showLogin", 1f);
                }
            }
            else
            {
                Invoke("showLogin", 1f);
            }
        }

        Firebase.Auth.FirebaseUser user;
       
        public UnityEvent OnFirebaseInitialized = new UnityEvent();
        private async void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
                var dependencyResult = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
                if (dependencyResult == Firebase.DependencyStatus.Available)
                {
                    app = Firebase.FirebaseApp.DefaultInstance;
                    OnFirebaseInitialized.Invoke();
                }
                else
                {
                    Debug.LogError("failed");
                }
            }
            else
            {
                Debug.LogError("already instantiate");
            }


            if (!FB.IsInitialized)
            {
                print(" Initialize the Facebook SDK");
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                print(" Already initialized, signal an app activation App Event");
                FB.ActivateApp();
            }
        }
        public void loadSceneWithName(string scenename)
        {

            Debug.Log("sceneName to load: " + scenename);
            SceneManager.LoadScene(scenename);
            Debug.Log("sceneName to load: " + scenename);
        }

        public InputField signUpEmail;
        public InputField signUpPassword;
        public void signUP()
        {
            if (!string.IsNullOrEmpty(signUpEmail.text) && !string.IsNullOrEmpty(signUpPassword.text))
            {

                signUp(signUpEmail.text, signUpPassword.text);
            }
            else
            {
                  msg = "يجب ملء الحقول";
                  popUpFlag = true;

            }
        }
       
        public InputField resetPasswordEmail;
        public void resetPassword()
        {
            print(resetPasswordEmail.text);
            if (!string.IsNullOrEmpty(resetPasswordEmail.text))
            {

                SendPasswordResetEmail(resetPasswordEmail.text);
            }
            else
            {
                msg = "يجب ملء الحقول";
                popUpFlag = true;
            }
        }
        private void SendPasswordResetEmail(string email)
        {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread((authTask) => {
                if (authTask.IsCanceled)
                {
                    Debug.Log("Password reset was canceled.");
                }
                else if (authTask.IsFaulted)
                {
                    Debug.Log("Password reset encountered an error.");
                    Debug.Log(authTask.Exception.ToString());
                }
                else if (authTask.IsCompleted)
                {
                    Debug.Log("Password reset successful!");
                }
            });
        }
        public InputField signInEmail;
        public InputField signInPassword;
        public void signIN()
        {
            if (!string.IsNullOrEmpty(signInEmail.text) && !string.IsNullOrEmpty(signInPassword.text)) {

                loadingObject.SetActive(true);
                PlayerPrefs.SetString("MallEmail", signInEmail.text);
            signIn(signInEmail.text, signInPassword.text);
            }
            else
            {
                msg = "يجب ملء الحقول";
                popUpFlag = true;

            }
        }
        private void signIn(string email, string password)
        {
            print("email : " + email);
            print("password : " + password);
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                 
                    Debug.LogError("SignIn was canceled.");
                    msg = "SignIn was canceled.";
                    popUpFlag = true;

                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInerror: " + task.Exception);
                    msg = "يوجد خطأ بالإيميل/الباسورد";

                    Debug.Log(task.Exception.InnerExceptions[0].InnerException.Message);
                    string messageException = task.Exception.InnerExceptions[0].InnerException.Message;
                    switch (messageException)
                    {
                        case "There is no user record corresponding to this identifier. The user may have been deleted.":
                            msg = "عذرا\nالبريد الإلكترونى ليس مسجل من قبل";
                            break;
                        case "The email address is badly formatted.":
                            msg = "عذرا\nيجب أن تكون صيغة البريد الإلكترونى \nemail@example.com ";
                            break;
                           
                        case "The password is invalid or the user does not have a password.":
                            msg = "عذرا\nيوجد خطأ بالباسورد ";
                            break;
                       
                        default:
                            msg = "حدث خطأ برجاء المحاولة مره أخرى";
                            break;
                    }
                    popUpFlag = true;
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
               // loadSceneWithName("mall");
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        }
        
        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }
        public void loginwithFacebook()
        {
            //get accessToken fro fb


            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(callback: AuthCallback);
        
        //FB.LogInWithReadPermissions(AuthCallback);

            

        }
        private void AuthCallback(ILoginResult result)
        {
            if (result.Error!=null)
            {
                Debug.Log(result.Error);
            }
            print(FB.IsLoggedIn);
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
               
                // Print current access token's User ID
                Debug.Log(aToken.UserId);
                // Print current access token's granted permissions
                signInwithFacebook(Facebook.Unity.AccessToken.CurrentAccessToken.UserId);
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }
            }
            else
            {
                Debug.Log("User cancelled login");
            }
        }
        private void signInwithFacebook(string accessToken)
        {
            Firebase.Auth.Credential credential =
            Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
            auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                //loadSceneWithName("mall");

            });
        }
        void loginwithTwitter()
        {
            //get accessToken fro fb

            // signInwithFacebook(string accessToken)

        }
        /*  private void signInwithTwitter(string accessToken)
          {
              Firebase.Auth.Credential credential =
              Firebase.Auth.TwitterAuthProvider.GetCredential(accessToken, secret);
              auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                  if (task.IsCanceled)
                  {
                      Debug.LogError("SignInWithCredentialAsync was canceled.");
                      return;
                  }
                  if (task.IsFaulted)
                  {
                      Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                      return;
                  }

                  Firebase.Auth.FirebaseUser newUser = task.Result;
                  Debug.LogFormat("User signed in successfully: {0} ({1})",
                      newUser.DisplayName, newUser.UserId);
                  loadScene("mall");
              });
          }*/

        private void signUp(string email, string password)
        {

            PlayerPrefs.SetString("MallEmail", email);
            loadingObject.SetActive(true);
            print(email);
            print(password);
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
                task =>
            {
                if (task.IsCanceled)
                {
                    msg = "signUp was canceled";
                    popUpFlag = true;
                    Debug.LogError("signUp was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                   
                     Debug.Log(task.Exception.InnerExceptions[0].InnerException.Message);
                    string messageException = task.Exception.InnerExceptions[0].InnerException.Message;
                    switch (messageException)
                        {
                        case "The email address is already in use by another account.":
                            msg = "عذرا\nالبريد الإلكترونى مسجل من قبل\nيرجى اختيار بريد إلكترونى أخر";
                            break;


                        case "The email address is badly formatted.":
                            msg = "عذرا\nيجب أن تكون صيغة البريد الإلكترونى email@example.com ";
                            break;
                        case "The given password is invalid.":
                            msg = "يجب ان يكون الباسورد مكون من 8 حروف علي الأقل";
                            break;
                        default:
                    msg = "حدث خطأ برجاء المحاولة مره أخرى";
                                break;
                        }
            popUpFlag = true;

            return;
                }
                /*
                signUPObject.SetActive(false);
                completeProfileObject.SetActive(true);
                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                                  newUser.DisplayName, newUser.UserId);
                newUser.TokenAsync(false).ContinueWithOnMainThread(
                //,
                //task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result)));
                getTokenTask => PlayerPrefs.SetString("EMallUserToken", getTokenTask.Result));
                //loadSceneWithName("mall");
              */
            });
        }
        public GameObject logInObject,signUPObject, completeProfileObject,popup,loadingObject;
        Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        bool finished = true;
        string token = "";
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
        public void StartFun()
        {
           
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(
                      "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
       
        public Task CreateUserWithEmailAsync(string email,string password)
        {
            Debug.Log(String.Format("Attempting to create user {0}...", email));
      //      DisableUI();

            return auth.CreateUserWithEmailAndPasswordAsync(email, password)
              .ContinueWithOnMainThread((task) =>
              {
        //          EnableUI();
                  if (LogTaskCompletion(task, "User Creation"))
                  {

                      print("creation is done");
                      var user = task.Result;
                      DisplayDetailedUserInfo(user, 1);
                      return UpdateUserProfileAsync("newDisplayName");
                  }
                  return task;
              }).Unwrap();
        }
        protected bool LogTaskCompletion(Task task, string operation)
        {
            bool complete = false;
            if (task.IsCanceled)
            {
                Debug.Log(operation + " canceled.");
            }
            else if (task.IsFaulted)
            {
                Debug.Log(operation + " encounted an error.");
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    string authErrorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        authErrorCode = String.Format("AuthError.{0}: ",
                          ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    }
                    Debug.Log(authErrorCode + exception.ToString());
                }
            }
            else if (task.IsCompleted)
            {
                Debug.Log(operation + " completed");
                complete = true;
            }
            return complete;
        }





        [System.Serializable]
        public class Response
        {
            public string full_name;
            public string address=null;
            public string phone_number;
            public string city;
            public bool is_complete;
        }

        void getData()
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
                print(idToken);  // Send token to your backend via HTTPS
                                 // ...
            });

            getData();
        }

        IEnumerator getUserData()
        {
            while (!finished)
            {
                yield return new WaitForSeconds(0.1f);

            }
            if (token != "")
            {
                PlayerPrefs.SetString("MallTokenId", token);
            }
            UnityWebRequest www = UnityWebRequest.Get(hostManager.domain + "api/profile/");
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            print(token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {

                generateToken();
                StartCoroutine(getUserData());
            //popup server error
            Debug.Log(www.error);
            }
            else
            {
                print(www.downloadHandler.text);
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                Parse(response);

            }




        }
        void Parse(Response response)
        {
            loadingObject = GameObject.FindGameObjectWithTag("loading");
            if (loadingObject != null)
            {
                loadingObject.SetActive(false);
            }
            PlayerPrefs.SetString("MallFullName", response.full_name);
            PlayerPrefs.SetString("MallAddress", response.address);
            PlayerPrefs.SetString("MallPhoneNumber", response.phone_number);

            if (response.is_complete)
            {
                PlayerPrefs.SetInt("MallIsCompleted", 1);
                loadSceneWithName("mall");
            }
            else
            {
               
                GameObject splash = GameObject.FindGameObjectWithTag("splash");

                splash.transform.GetChild(0).gameObject.SetActive(false);
                GameObject container = GameObject.FindGameObjectWithTag("login");

                container.transform.GetChild(0).gameObject.SetActive(false);


                container.transform.GetChild(1).gameObject.SetActive(false);

                container.transform.GetChild(3).gameObject.SetActive(true);
               
            }

            /*  logInObject.SetActive(false);
              signUPObject.SetActive(false);
              completeProfileObject.SetActive(true);
         */
        


    }
        IEnumerator showPopUp(string msg)
        {
            print("show popup");
            popup.SetActive(true);
            print("popup : " + popup.active);
            
            popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = msg;
            yield return new WaitForSeconds(0.0f);
        }




        protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
        {

            // loadSceneWithName("mall");
            string indent = new String(' ', indentLevel * 2);
            DisplayUserInfo(user, indentLevel);
            Debug.Log(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
            Debug.Log(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
            Debug.Log(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
            var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
            var numberOfProviders = providerDataList.Count;
            if (numberOfProviders > 0)
            {
                for (int i = 0; i < numberOfProviders; ++i)
                {
                    Debug.Log(String.Format("{0}Provider Data: {1}", indent, i));
                    DisplayUserInfo(providerDataList[i], indentLevel + 2);
                }
            }
            finished = false;
            Firebase.Auth.FirebaseAuth auth;
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            user = auth.CurrentUser;

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
            if (PlayerPrefs.GetInt("MallIsCompleted") == 1)
            {
                loadSceneWithName("mall");
            }
            else
            {
                StartCoroutine(getUserData());
            }
        }
        protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
        {

            PlayerPrefs.SetString("MallPhoto", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null);
            string indent = new String(' ', indentLevel * 2);
            var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };
            foreach (var property in userProperties)
            {
                if (!String.IsNullOrEmpty(property.Value))
                {
                    Debug.Log(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
                }
            }
        }
        public Task UpdateUserProfileAsync(string newDisplayName = null)
        {
            if (auth.CurrentUser == null)
            {
                Debug.Log("Not signed in, unable to update user profile");
                return Task.FromResult(0);
            }
           // displayName = newDisplayName ?? displayName;
            Debug.Log("Updating user profile");
         //   DisableUI();
            return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
            {
          //      DisplayName = displayName,
                PhotoUrl = auth.CurrentUser.PhotoUrl,
            }).ContinueWithOnMainThread(task => {
         //       EnableUI();
                if (LogTaskCompletion(task, "User profile"))
                {
                    DisplayDetailedUserInfo(auth.CurrentUser, 1);
                }
            });
        }
        
    }
}