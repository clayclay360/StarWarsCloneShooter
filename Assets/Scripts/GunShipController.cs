using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShipController : MonoBehaviour
{
    [SerializeField]
    GameObject Player, CloneTrooper;

    [SerializeField]
    private Transform[] ClonePositions;

    public bool releaseTroops;

    public GameController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<GameController>().GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transport()
    {
        if(controller.waveNumber == 1)
        {
            Instantiate(Player, ClonePositions[4].position,Quaternion.identity);
        }
        else
        {
            for(int i = 0; i < 4; i++)
            {
                Instantiate(CloneTrooper, ClonePositions[i].position,Quaternion.identity);
            }
        }
    }
}
