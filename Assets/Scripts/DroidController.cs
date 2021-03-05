using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroidController : MonoBehaviour
{
    [SerializeField]
    GameController gamecontroller;

    [SerializeField]
    GameObject blueLaserBlast, blastPosition;

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
    private GameObject[] Clone;

    private Collider droidCollider;
    public float[] ClonesMagnitudeofDistance;
    public bool died;

    bool fireReady;
    int deathNumber;

    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gamecontroller = FindObjectOfType<GameController>().GetComponent<GameController>(); // get gamecontroller
        gamecontroller.currentNumberofDroids++; //one the currentNumberofDroids
        animator = GetComponent<Animator>(); // get animator
        agent = GetComponent<NavMeshAgent>(); // get agent
        fireReady = true; //fireReady is true
        droidCollider = GetComponent<Collider>(); //get collider
        Clone = GameObject.FindGameObjectsWithTag("Clone"); //create a clone object and allow it to find all the gameobjects with the tag "UnAssignedClone"
    } 

    // Update is called once per frame
    void Update()
    {
        if (!died && gamecontroller.currentNumberofClones != 0) //run if not died and there are clones still alive
        {
            CalculateDistanceofClones(); // function of CalculateDistanceofClones
            CalculateShortestDistance(); //function of CalculateShortestDistance
            CheckTargetsDistance(); //function CheckTargetsDistance
            FindClones(); //function of FindClones
        }
        CheckGameOver();
    }

    void CalculateDistanceofClones() // CalculateDistanceofClones function
    {
        for (int i = 0; i < gamecontroller.currentNumberofClones; i++) //run this loop depending on the number of the current # of clones
        {
            if (Clone[i] != null)
            {
                ClonesMagnitudeofDistance[i] = Vector3.Distance(Clone[i].transform.position, transform.position); // get the distance of the droid to each clone
            }
        }
    }

    void FindClones() //this coroutine is for the an issue that was happening with the droids, everytime a clone died they would malfunction
    {
        Clone = GameObject.FindGameObjectsWithTag("Clone"); // find all clones with the tag clone
    }

    void CalculateShortestDistance() // CalculateShortestDistance function
    {
        int shortestDis = 0; // variable shortestDis

        for (int currentDist = 0; currentDist < gamecontroller.currentNumberofClones; currentDist++) //run this loop depending on the number of the current # of clones
        {
            if(ClonesMagnitudeofDistance[currentDist] > ClonesMagnitudeofDistance[shortestDis] && Clone[currentDist] != null) //check to see if the shortestDist is shorter than the currentDist, pretty much check to see if there is a distance shorter from the distances from the previous clone and clone is not null
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

    void CheckTargetsDistance() //CheckTargetDistance function
    {
        animator.SetFloat("Idle_Move", agent.velocity.magnitude); //set float Idle_move to the agents velocity

        if (Clone[closestTarget] != null && Vector3.Distance(Clone[closestTarget].transform.position, transform.position) <= attackRaduis) // if the closest clone is within the attack radius and target is not null
        {
            transform.LookAt(Clone[closestTarget].transform.position + offset); // look at the clone

            if (fireReady) //if fireReady is true
            {
                StartCoroutine(CoolDown()); //start CoolDown
                fireReady = false; //fireReady is false
            }
        }
        else if (Clone[closestTarget] != null && Vector3.Distance(Clone[closestTarget].transform.position, transform.position) <= followRaduis) //if the droid is within the follow raduis
        {
            agent.stoppingDistance = attackRaduis; //stopping distance equals attack radius
            agent.destination = Clone[closestTarget].transform.position; //droid move toward closest clone
        }
    }

    IEnumerator CoolDown() //CoolDown function
    {
        animator.SetTrigger("Shoot"); // set trigger shoot
        Instantiate(blueLaserBlast, blastPosition.transform.position, transform.rotation); //instantiate blue blast from droid  blast position and rotation
        yield return new WaitForSeconds(coolDown); // wait for a certain amount of seconds (cooldown time)
        fireReady = true; // fireReady
    }

    private void OnParticleCollision(GameObject other) //called everytime a particle hits the gameobjects collider
    {
        if (other.CompareTag("redBlast")) { 
            Destroy(other); //destroy particle
            health -= 50; //decrease the droids health by 50%
            if (health == 0) //if the droids health is 0 or less
            {
                Destroy(gameObject, timeOfDeath); //destroy this gameobject in the timeOfDeath 
                died = true; //died equals true, allows the game to acknowledge that the droid is died
                deathNumber = Random.Range(1, 4); //spit a random number for the death animation
                animator.SetInteger("Death", deathNumber); //set the integer of what animation is to be played
                gamecontroller.currentNumberofDroids--; //one less droid
                gamecontroller.enemyKills++; // enemykills goes up by one
                droidCollider.enabled = false; //disable the collider (so the player and droid arent hitting it when they are trying to shoot)
            }
        }

        if (other.CompareTag("blueBlast")){
            Destroy(other);
        }
    }

    private void OnMouseOver() //when the mouse is over the droid
    {
        gamecontroller.cursorTextureNumber = 1; //change the cursors texture to texture 1 in the cursortexturenumber array
        gamecontroller.CursorSettings(); //run cursorsettings function
    }

    private void OnMouseExit() //when the mouse exists the droid
    {
        gamecontroller.cursorTextureNumber = 0; //change the cursors texture to texture 0 in the cursortexturenumber array
        gamecontroller.CursorSettings(); //run cursorsettings function
    }

    void CheckGameOver()
    {
        if (gamecontroller.gameOver) //if game over self destruct
        {
            StartCoroutine(SelfDestruct()); // start the self destruct coroutine
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1); //wait one sec
        gamecontroller.currentNumberofDroids--; //currentnumber of droids equal - 1
        Destroy(gameObject); //destroy gameobject
    }

}
