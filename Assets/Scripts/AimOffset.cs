using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimOffset : MonoBehaviour
{

    public Transform Gun,Aim;

    // Start is called before the first frame update
    void Start()
    {
        Gun.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Aim.transform.position = new Vector3(ray.);

        /*if (Physics.Raycast(ray,out hit))
        {
            Debug.Log("On");
            Aim.transform.position = new Vector3(hit.point.x,hit.point.y,hit.point.z);
            Debug.DrawRay(Gun.transform.position, new Vector3(hit.point.x, hit.point.y, hit.point.z) * 10, Color.red);
        }*/

        //Debug.DrawRay(Gun.transform.position,Vector3.forward*10,Color.red);
    }
}
