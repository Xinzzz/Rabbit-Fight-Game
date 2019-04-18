using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyPartical : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    
}
