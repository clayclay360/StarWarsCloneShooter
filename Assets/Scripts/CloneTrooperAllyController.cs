using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CloneTrooperAllyController : MonoBehaviour
{
    [SerializeField]
    GameController gamecontroller;

    [SerializeField]
    GameObject redLaserBlast, blastPosition;

    [SerializeField]
    int closestTarget;

    [SerializeField]
    float followRaduis, attackRaduis;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float coolDown;

    [SerializeField]
    private int health = 100;

    [SerializeField]
    private int timeOfDeath = 5;


    [SerializeField]
    private GameObject[] Droid;

    private Collider cloneColider;
    public float[] DroidMagnitudeofDistance;
    public bool died;

    bool fireReady;
    int deathNumber;

    NavMeshAgent agent;
    Animator animator;
    DroidController droidController;

    // Start is called before the first frame update
    void Start()
    {
        gamecontroller = FindObjectOfType<GameController>().GetComponent<GameController>(); // get gamecontroller
        gamecontroller.currentNumberofClones++;
        animator = GetComponent<Animator>(); // get animator
        agent = GetComponent<NavMeshAgent>(); // get agent
        fireReady = true; //fireReady is true
        cloneColider = GetComponent<Collider>();
        StartCoroutine(FindDroids());
    }

    // Update is called once per frame
    void Update()
    {
        if (!died && Droid.Length != 0) 
        {
            CalculateDistanceofDroids(); // function of CalculateDistanceofDroids
            CalculateShortestDistance(); //function of CalculateShortestDistance
            CheckTargetsDistance(); //function CheckTargetsDistance
            //FindDroids(); //function of FindDroids
        }
    }

    IEnumerator FindDroids()
    {
        yield return new WaitForSeconds(0.1f);
        Droid = GameObject.FindGameObjectsWithTag("Enemy"); //create a droid object and allow it to find all the gameobjects with the tag "UnAssignedClone"
        StartCoroutine(FindDroids());
    }

    void CalculateDistanceofDroids() // CalculateDistanceofDroids function
    {
        for (int i = 0; i < Droid.Length; i++) //run this loop depending on the number of the current # of droids
        {
            if (Droid[i] != null)
            {
                DroidMagnitudeofDistance[i] = Vector3.Distance(Droid[i].transform.position, transform.position); // get the distance of the droid to each clone
            }
        }
    }

    void CalculateShortestDistance() // CalculateShortestDistance function
    {
        int shortestDis = 0; // variable shortestDis

        for (int currentDist = 0; currentDist < Droid.Length; currentDist++) //run this loop depending on the number of the current # of droids
        {
            if (Droid[shortestDis] != null) //if droid is not null
            {
                DroidController droidController = Droid[shortestDis].GetComponent<DroidController>(); //get the droids controller script

                if (DroidMagnitudeofDistance[currentDist] > DroidMagnitudeofDistance[shortestDis] && !droidController.died) //check to see if the shortestDist is shorter than the currentDist, pretty much check to see if there is a distance shorter from the distances from the previous clone and droid is not died (this fixes a previous issue of the clones still calculating the shortest droid that was died but not null
                {
                    closestTarget = shortestDis; // if the shortestDis is shorter, closestTarget equals shortestDis
                }
                else //else 
                {
                    shortestDis = currentDist; //shortestDis equals currentDist
                    closestTarget = shortestDis; // closestTarget
                }
            }
        }
    }

    void CheckTargetsDistance() //CheckTargetDistance function
    {
        animator.SetFloat("Idle_Move", agent.velocity.magnitude); //set float Idle_move to the agents velocity

        if (Droid[closestTarget] != null) //if the closesttarget is not null or died
        {
            droidController = Droid[closestTarget].GetComponent<DroidController>(); //get the closest droids controller

            if (Vector3.Distance(Droid[closestTarget].transform.position, transform.position) <= attackRaduis && !droidController.died) // if the closest clone is within the attack radius and is not died
            {
                transform.LookAt(Droid[closestTarget].transform.position + offset); // look at the clone

                if (fireReady) //if fireReady is true
                {
                    StartCoroutine(CoolDown()); //start CoolDown
                    fireReady = false; //fireReady is false
                }
            }
            else if (Vector3.Distance(Droid[closestTarget].transform.position, transform.position) <= followRaduis) //if the droid is within the follow raduis
            {
                agent.stoppingDistance = attackRaduis; //stopping distance equals attack radius
                agent.destination = Droid[closestTarget].transform.position; //droid move toward closest clone
            }
        }
    }

    IEnumerator CoolDown() //CoolDown function
    {
        animator.SetTrigger("Shoot"); // set trigger shoot
        Instantiate(redLaserBlast, blastPosition.transform.position, transform.rotation); //instantiate blue blast from droid  blast position and rotation
        yield return new WaitForSeconds(coolDown); // wait for a certain amount of seconds (cooldown time)
        fireReady = true; // fireReady
    }

    private void OnParticleCollision(GameObject other) //called everytime a particle hits the gameobjects collider
    {
        if (other.CompareTag("blueBlast")) {
            Destroy(other); //destroy particle
            health -= 50; //decrease the droids health by 50%
            if (health == 0) //if the droids health is 0 or less
            {
                Destroy(gameObject, timeOfDeath); //destroy this gameobject in the timeOfDeath 
                died = true; //died equals true, allows the game to acknowledge that the droid is died
                deathNumber = Random.Range(1, 4); //spit a random number for the death animation
                animator.SetInteger("Death", deathNumber); //set the integer of what animation is to be played
                cloneColider.enabled = false;
                gamecontroller.currentNumberofClones--; //one less droid
            }
        }
    }
}

//A way to make the clones faster is to make sure there only checking for droids that are alive