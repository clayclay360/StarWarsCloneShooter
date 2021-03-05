using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPointLookAt : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.position);
    }
}
