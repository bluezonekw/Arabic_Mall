using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MallLoader : MonoBehaviour
{
    public Transform T1;
    bool[] sceneloaded= { false, false, false, false, false, false, false, false, false, false,false} ;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(T1.position.z);

        if (T1.position.z < 10 )
        {
            try
            {
                SceneManager.UnloadSceneAsync("03");
                sceneloaded[03] = false;
            }
            catch
            {

            }
            if (!sceneloaded[01])
            {
                SceneManager.LoadScene("01", LoadSceneMode.Additive);
                sceneloaded[01] = true;
            }
            if (!sceneloaded[02])
            {
                SceneManager.LoadScene("02", LoadSceneMode.Additive);
                sceneloaded[02] = true;

            }
        }
        else
        if (T1.position.z > 10 && T1.position.z < 90)
        {
            try
            {
                SceneManager.UnloadSceneAsync("01");
                sceneloaded[01] = false;


            }
            catch
            {

            }

            try
            {
                SceneManager.UnloadSceneAsync("04");

                sceneloaded[04] = false;

            }
            catch
            {

            }

            if (!sceneloaded[03])
            {
                SceneManager.LoadScene("03", LoadSceneMode.Additive);
                sceneloaded[03] = true;


            }
            if (!sceneloaded[02])
            {

                SceneManager.LoadScene("02", LoadSceneMode.Additive);
                sceneloaded[02] = true;

            }
        }
        else
            if (T1.position.z > 90 && T1.position.z < 170)
        {
            try
            {
                SceneManager.UnloadSceneAsync("02");
                sceneloaded[02] = false;

            }
            catch
            {

            }

            try
            {
                SceneManager.UnloadSceneAsync("05");
                sceneloaded[05] = false;

            }
            catch
            {

            }
            if (!sceneloaded[04])
            {
                SceneManager.LoadScene("04", LoadSceneMode.Additive);

                sceneloaded[04] = true;

            }
            if (!sceneloaded[03])
            {

                SceneManager.LoadScene("03", LoadSceneMode.Additive);
                sceneloaded[03] = true;

            }
        }


        else
            if (T1.position.z > 170 && T1.position.z < 250)
        {
            try
            {
                SceneManager.UnloadSceneAsync("03");
                sceneloaded[03] = false;

            }
            catch
            {

            }

            try
            {
                SceneManager.UnloadSceneAsync("06");
                sceneloaded[06] = false;

            }
            catch
            {

            }

            if (!sceneloaded[05])
            {
                SceneManager.LoadScene("05", LoadSceneMode.Additive);

                sceneloaded[05] = true;

            }
            if (!sceneloaded[04])
            {

                SceneManager.LoadScene("04", LoadSceneMode.Additive);
                sceneloaded[04] = true;

            }
        }

        else
            if (T1.position.z > 250 && T1.position.z < 330)
        {
            try
            {
                SceneManager.UnloadSceneAsync("04");
                sceneloaded[04] = false;

            }
            catch
            {

            }

            try
            {
                SceneManager.UnloadSceneAsync("07");
                sceneloaded[07] = false;

            }
            catch
            {

            }
            if (!sceneloaded[06])
            {

                SceneManager.LoadScene("06", LoadSceneMode.Additive);
                sceneloaded[06] = true;

            }
            if (!sceneloaded[05])
            {

                SceneManager.LoadScene("05", LoadSceneMode.Additive);
                sceneloaded[05] = true;

            }
        }


        else
            if (T1.position.z > 330 && T1.position.z < 410)
        {
            try
            {
                SceneManager.UnloadSceneAsync("05");
                sceneloaded[05] = false;

            }
            catch
            {

            }

            try
            {
                SceneManager.UnloadSceneAsync("08");
                sceneloaded[08] = false;

            }
            catch
            {

            }

            if (!sceneloaded[07])
            {
                SceneManager.LoadScene("07", LoadSceneMode.Additive);
                sceneloaded[07] = true;


            }
            if (!sceneloaded[06])
            {
                SceneManager.LoadScene("06", LoadSceneMode.Additive);
                sceneloaded[06] = true;


            }
        }

        else
            if (T1.position.z > 410 && T1.position.z < 490)
        {
            try
            {
                SceneManager.UnloadSceneAsync("06");
                sceneloaded[06] = false;

            }
            catch
            {

            }

            try
            {
                SceneManager.UnloadSceneAsync("09");
                sceneloaded[09] = false;

            }
            catch
            {

            }

            if (!sceneloaded[08])
            {
                SceneManager.LoadScene("08", LoadSceneMode.Additive);

                sceneloaded[08] = true;

            }
            if (!sceneloaded[07])
            {
                SceneManager.LoadScene("07", LoadSceneMode.Additive);

                sceneloaded[07] = true;

            }
        }


        else
            if (T1.position.z > 490 && T1.position.z < 570)
        {
            try
            {
                SceneManager.UnloadSceneAsync("07");
                sceneloaded[07] = false;

            }
            catch
            {

            }

            try
            {
                SceneManager.UnloadSceneAsync("10");
                sceneloaded[10] = false;

            }
            catch
            {

            }

            if (!sceneloaded[09])
            {
                SceneManager.LoadScene("09", LoadSceneMode.Additive);
                sceneloaded[09] = true;

            }
            if (!sceneloaded[08])
            {
                SceneManager.LoadScene("08", LoadSceneMode.Additive);
                sceneloaded[08] = true;

            }
        }

        else
            if (T1.position.z > 570 )
        {
            try
            {
                SceneManager.UnloadSceneAsync("08");
                sceneloaded[08] = false;

            }
            catch
            {

            }

            if (!sceneloaded[10])
            {
                SceneManager.LoadScene("10", LoadSceneMode.Additive);

                sceneloaded[10] = true;

            }
        }
    }
}
