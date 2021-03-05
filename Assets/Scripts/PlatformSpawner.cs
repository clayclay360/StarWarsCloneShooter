using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platForm;

    // Start is called before the first frame update
    void Start()
    {
        platForm.GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("LOL");
            Instantiate(platForm, new Vector3(0, 0, transform.position.z + 10), Quaternion.identity);
        }
    }
}
