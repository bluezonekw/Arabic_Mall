using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.Extensions
{
    public class NextscreenPlay : MonoBehaviour
    {
         ScrollSnap scrollSnap;
        bool Paused;
        // Start is called before the first frame update
        void Start()
        {
            scrollSnap = GetComponent<ScrollSnap>();
        }
        public void PuseItem()
        {
            Paused = !Paused;
        }
        // Update is called once per frame
        void Update()
        {
           
        }
       
    }
}
