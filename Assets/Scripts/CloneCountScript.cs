using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneCountScript : MonoBehaviour
{

    public GameObject[] Clone;

    public int cloneNumber;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < Clone.Length; i++)
        //{
        //    GameObject CloneObject = GameObject.FindGameObjectWithTag("UnAssignedClone");

        //    if (Clone[i] == null)
        //    {
        //        Clone.SetValue(CloneObject, cloneNumber);
        //        CloneObject.gameObject.tag = "AssignedClone";

        //        cloneNumber++;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //Clone = GameObject.FindGameObjectsWithTag("Clone");
        //Clone.SetValue(cloneEnemy, cloneNumber);
        //cloneNumber++;
    }
}
