
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

    public class authControllerInMallScene : MonoBehaviour
    {
        bool popUpFlag = false;
        bool loginFlag = false;
        string msg="";
        private void Update()
        {
            if (popUpFlag)
            {
                loadingObject.SetActive(false);
                StartCoroutine(showPopUp(msg));
                popUpFlag = false;
            }
            if (loginFlag)
            {
                StartCoroutine(updateEmail());
                loginFlag = false;
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
        private static authControllerInMallScene instance;
        public static authControllerInMallScene Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new authControllerInMallScene();
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
            loadingObject.SetActive(true);
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
        void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            Firebase.Auth.FirebaseUser user = null;
            //senderAuth.SignOut();
            if (senderAuth != null)
            {
                userByAuth.TryGetValue(senderAuth.App.Name, out user);

            }
            else
            {

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

                    loadingObject.SetActive(true);
                    Firebase.Auth.FirebaseAuth auth;
                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
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
            if (!string.IsNullOrEmpty(signUpEmail.text) &&! string.IsNullOrEmpty(signUpPassword.text))

                signUp(signUpEmail.text, signUpPassword.text);
        }
       
        public InputField resetPasswordEmail;
        public void resetPassword()
        {
            print(resetPasswordEmail.text);
            if (!string.IsNullOrEmpty(resetPasswordEmail.text))

                SendPasswordResetEmail(resetPasswordEmail.text);
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
            loadingObject.SetActive(true);
            if (!string.IsNullOrEmpty(signInEmail.text) && !string.IsNullOrEmpty(signInPassword.text)) { 

                PlayerPrefs.SetString("MallEmail", signInEmail.text);
            signIn(signInEmail.text, signInPassword.text);
        }
        }
        private void signIn(string email, string password)
        {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            print("email : " + email);
            print("password : " + password);
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
                    msg = "SignInerror: Password or email is not correct" ;
                    popUpFlag = true;
                    return;
                }
                loginFlag = true;
                Firebase.Auth.FirebaseUser newUser = task.Result;
               // loadSceneWithName("mall");
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);

               // StartCoroutine(updateEmail());
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
                    msg = "SignUp error: email is founded ";
                    popUpFlag = true;

                   // showPopUp("SignUp error: " + task.Exception);

                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }
               
            });
        }
        public GameObject logInObject,signUPObject, completeProfileObject,popup,loadingObject;
        Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        bool finished = false;
        string token = "";
       
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
        


        IEnumerator getUserData()
        {
            if (!finished)
            {
                yield return new WaitForSeconds(1.0f);

            }
            UnityWebRequest www = UnityWebRequest.Get(hostManager.domain + "api/profile/");
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", token);
            print(token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
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
            loadingObject.SetActive(false);
            PlayerPrefs.SetString("MallFullName", response.full_name);
            PlayerPrefs.SetString("MallAddress", response.address);
            PlayerPrefs.SetString("MallPhoneNumber", response.phone_number);

            if (response.is_complete)
            {
                loadSceneWithName("mall");
            }
            else
            {
                logInObject.SetActive(false);
                signUPObject.SetActive(false);
                completeProfileObject.SetActive(true);
            }


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

            // StartCoroutine(getUserData());
            loginFlag = true;
        }
        public GameObject data;
        public GameObject loginPanel;
        IEnumerator updateEmail()
        {
            print("updateEmail");
            loadingObject.SetActive(false);
            data.SetActive(false);
            data.SetActive(true);
            loginPanel.SetActive(false);
            yield return new WaitForSeconds(0.0f);
        }
        protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
        {
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