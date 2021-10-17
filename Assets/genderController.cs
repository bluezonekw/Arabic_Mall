using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class genderController : MonoBehaviour
{
    public Actions action;
    public GameObject male, female;
    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("MallGender"))
        {
            PlayerPrefs.SetInt("MallGender", 0);
        }
        if (PlayerPrefs.GetInt("MallGender") == 0)
        {
            male.SetActive(true);
            female.SetActive(false);

        }
        else if (PlayerPrefs.GetInt("MallGender") == 1)
        {

            male.SetActive(false);
            female.SetActive(true);
        }
    }
    public void changeGender(int index)
    {
        print(index);
        if (index == 0)
        {
            male.SetActive(true); 
            female.SetActive(false);

        }
        else if (index == 1)
        {
            male.SetActive(false);
            female.SetActive(true);
        }
        PlayerPrefs.SetInt("MallGender", index);
        if (action != null)
        {
            action.changeGender();
        }
    }
}
