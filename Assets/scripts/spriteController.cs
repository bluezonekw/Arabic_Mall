using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteController : MonoBehaviour
{
    public GameObject menu;
    public GameObject map;
    public Sprite menuon, menuoff;
    public void mapClicked()
    {
        print(!map.activeSelf);
        map.SetActive(!map.activeSelf);
    }
    public void menuBtn()
    {
        if (menu.active)
        {
            menu.SetActive(false);

            this.GetComponent<Image>().sprite = menuoff;
        }
        else
        {
            menu.SetActive(true);

            this.GetComponent<Image>().sprite = menuon;

        }
    }
    public void changeSprite(Sprite icon)
    {
        this.GetComponent<Image>().sprite = icon;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
