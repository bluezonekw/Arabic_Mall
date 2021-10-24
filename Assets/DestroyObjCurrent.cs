using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjCurrent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
