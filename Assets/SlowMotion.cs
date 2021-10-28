using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{ bool stay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y > 3)
        {
            stay = false; 
        }
        else
        {
            stay = true;
        }
        if (!stay)
        {
            this.transform.Translate(Vector3.down * Time.deltaTime*2.0f, Space.Self);
        }
        else
        {
            Destroy(this.gameObject, 30.0f);
        }
    }
}
