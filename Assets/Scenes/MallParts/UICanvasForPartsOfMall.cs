using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterAssets
{
    public class UICanvasForPartsOfMall : UICanvasControllerInput 
    {
        public static StarterAssetsInputs uicanvas;
        // Start is called before the first frame update
        void Start()
        {
            try
            {

                uicanvas = GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>();
                print(uicanvas);

                starterAssetsInputs = uicanvas;
            }
            catch
            {
                print("›‘·       ");
            }

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}