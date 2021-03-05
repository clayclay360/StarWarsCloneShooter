using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBlasterOffset : MonoBehaviour
{

    //This script is made because unity is acting trash so now I have to make the transform equal to an object inside the arm so it looks more realistic

    [SerializeField]
    public Transform Offset;

    // Start is called before the first frame update
    void Start()
    {
        Offset.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Offset.transform.position;
    }
}
