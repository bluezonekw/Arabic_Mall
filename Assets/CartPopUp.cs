using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CartPopUp : MonoBehaviour
    {
        public GameObject PopExitShop,Male,Female;
        public UIVirtualJoystick sai;
   public Avatar MaleAvatar, FemaleAvater;
    public Animator a1;
     // Start is called before the first frame update
    void Start()
        {
            PopExitShop.SetActive(false);
       
    }
    private void Awake()
    {
        try
        {
            if (ApiClasses.Login.data.original.user.gander == 0)
            {

                Male.SetActive(true);
                a1.avatar = MaleAvatar;

            }
            else
            if (ApiClasses.Login.data.original.user.gander == 1)
            {

                Female.SetActive(true);
                a1.avatar = FemaleAvater;
            }
        }
        catch
        {
            try
            {
                if (ApiClasses.Register.data.user.gander == "0")
                {

                    Male.SetActive(true);
                    a1.avatar = MaleAvatar;
                }
                else
           if (ApiClasses.Register.data.user.gander == "1")
                {

                    Female.SetActive(true);
                    a1.avatar = FemaleAvater;
                }
            }
            catch
            {
                Male.SetActive(true);
                a1.avatar = MaleAvatar;
            }
        }
        
    }
    public void ResetMove()
        {
          sai.magnitudeMultiplier=1;
        }
        public void DestroyPop()
        {
            PopExitShop.SetActive(false);
        }
        // Update is called once per frame
        void Update()
        {
            if (CheckEnterShop.ExitShop)
            {
                PopExitShop.SetActive(true);
               sai.magnitudeMultiplier=0;

                CheckEnterShop.ExitShop = false;
            }

        }
    }
