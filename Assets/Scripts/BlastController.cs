using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject != null) //if the gameobject isnt already destroyed
        {
            Destroy(gameObject, 2); // Destory gameObject in one second
        }
    }
}
