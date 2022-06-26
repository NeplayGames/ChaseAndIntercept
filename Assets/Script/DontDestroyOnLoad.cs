using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
   
    void Start()
    {
      
        if (FindObjectsOfType<DontDestroyOnLoad>().Length == 2)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }

}
