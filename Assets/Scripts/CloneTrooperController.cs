using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CloneTrooperController : MonoBehaviour
{
    [SerializeField]
    private Animator CT_Armature;

    [SerializeField]
    private GameObject target, redLaserBlast, blastPosition;

    [SerializeField]
    private float fireRaduis, coolDown;

    [SerializeField]
    private Vector3 targetOffset;

    [SerializeField]
    private GameController gamecontroller;

    [SerializeField]
    private int health;

    [SerializeField]
    private float timeOfDeath = 1.5f;

    [SerializeField]
    private Slider healthbar;

    private bool onMove, fireReady, died;
    private int deathNumber;
    private Animator animator;
    private NavMeshAgent agent;
    private DroidController droidController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //get the animator
        agent = GetComponent<NavMeshAgent>(); //get the nav agent
        CT_Armature.GetComponent<Animator>(); //get the rig animator
        gamecontroller = FindObjectOfType<GameController>().GetComponent<GameController>(); //get the gamecontroller
        healthbar = FindObjectOfType<Slider>().GetComponent<Slider>(); //find and get the slider in the scene
        gamecontroller.currentNumberofClones += 1; //add to the number of clones by one
        fireReady = true; //fireReady equalls true
    }

    // Update is called once per frame
    void Update()
    {
        if (!died) //if not died
        {
            playerMovement(); //playerMovement Function
            findTarget(); //findEnemy Funtion
            moveOrFire(); //moveOrFire Function
            UpdateHealth(); //updatehealth Function
        }
    }

    // function to player's movement
    void playerMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //create a ray that is position to the main camera and point out at the direction of the camera   
        RaycastHit hit; //create a output of whatever the ray hits

        if (Input.GetButtonDown("Fire1"))   //if the player press's down on the left mouse button
        {
            if (Physics.Raycast(ray, out hit))  //get the ray that we created and get the output of hit
            {
                if (hit.collider.CompareTag("Environment")) //if the output has a collider tag named Environment
                {
                    agent.SetDestination(new Vector3(hit.point.x, hit.point.y, hit.point.z)); //go to the point where the mouse was clicked
                    target = null; //set target to null
                    agent.stoppingDistance = 0; //reset stopping distance to 0
                    agent.isStopped = false; //allow the agent to move
                }
            }
        }

        if (agent.velocity.magnitude > 0) //if the players velocity magnitude is greater than 0
        {
            onMove = true; //let the bool be true
        }
        else
        {
            onMove = false; //let the bool be false
        }

        CT_Armature.SetBool("onMove", onMove); //get the bool OnMove and assign it to onMove
        animator.SetFloat("Vertical", agent.velocity.magnitude); //get the float Vertical and set it to the player's magnitude
    }

    // function to find target
    void findTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //create a ray that is position to the main camera and point out at the direction of the camera   
        RaycastHit hit; //create a output of whatever the ray hits

        if (Input.GetButtonDown("Fire1")) //if the player press's down on the left mouse button
        {
            if (Physics.Raycast(ray, out hit)) //get the ray that we created and get the output of hit
            {
                if (hit.collider.CompareTag("Enemy")) //if the output has a collider tag named Enemy
                {
                    target = hit.collider.gameObject; //assign target to the collider that was hit
                    droidController = target.GetComponent<DroidController>(); //get the droids controller script
                    agent.isStopped = false; //allow the agent to move
                }
            }
        }
    }

    //function of moveOrFire
    void moveOrFire()
    {
        if(target != null) //if target isnt null
        {
            if(Vector3.Distance(transform.position, target.transform.position) < fireRaduis) //if the distance is less than 10 units
            {
                transform.LookAt(target.transform.position + targetOffset); //look at the enemy + the offset
                fireAtEnemy(); //run the function fireAtEnemy
            }
            else
            {
                moveTowardEnemy(); //run the function moveTowardsEnemy
            }
        }
    }
    
    //fundtion of fireEnemy
    void fireAtEnemy()
    {
        if (fireReady) //if fireReady is true
        {
            fireReady = false; //fireReady equals false
            StartCoroutine(fireRifle()); //start coroutine fireRifle
        }
    }

    //function of moveTowardsEnemy
    void moveTowardEnemy()
    {
        agent.SetDestination(target.transform.position); //set destination to targets position
        agent.stoppingDistance = fireRaduis; //the stopping distance equals the fire raduis
    }

    //coroutine of fireRifle
    IEnumerator fireRifle()
    {
        if (!droidController.died)
        {
            agent.velocity = Vector3.zero; //come to a full stop when shooting
            agent.isStopped = true; //stop the agent from moving so it can fire (this fixes any issue with the clone shooting and trying to go to a previous destination hit)
            animator.SetTrigger("Shoot"); //set trigger shoot
            Instantiate(redLaserBlast, blastPosition.transform.position, blastPosition.transform.rotation); //instantiate a fire blast in the position and direction of the blast position and players rotation
            yield return new WaitForSeconds(coolDown); //wait for a fixed amount of seconds
            fireReady = true; //fireReady equall's true
        }
        else
        {
            fireReady = true; //fireReady equall's true even if droid is died (solves issue previous problem if droid is died or not we want fireReady to be true)
        }
    }
    // function of updatehealth
    void UpdateHealth()
    {
        healthbar.value = health; //health slider value equals players health
        if (gamecontroller.regenHealth)
        {
            StartCoroutine(RegenerateHealth()); //start coroutine
            gamecontroller.regenHealth = false; //set to false
        }
    }

    IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(1);
        while(health != 100)
        {
            yield return new WaitForSeconds(.2f); //wait .2 seconds
            health += 5; //plus equal 5
        }
    }

    private void OnParticleCollision(GameObject other) //called when a particle hit the gameobjects collider
    {
        if (other.CompareTag("blueBlast"))
        {
            Destroy(other); //destroy particle
            health -= 10; //decrease the droids health by 50%
            if (health == 0) //if the droids health is 0 or less
            {
                Destroy(gameObject, timeOfDeath); //destroy this gameobject in the timeOfDeath 
                died = true; //died equals true, allows the game to acknowledge that the droid is died
                deathNumber = Random.Range(1, 4); //spit a random number for the death animation
                animator.SetInteger("Death", deathNumber); //set the integer of what animation is to be played
                gamecontroller.currentNumberofClones--; //one less droid
            }
        }
    }
}
