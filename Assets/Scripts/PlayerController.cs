using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anime;
    CharacterController controller;
    Vector3 direction;

    [SerializeField]
    private float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        anime.SetFloat("Vertical", vertical);
        anime.SetFloat("Horizontal", horizontal);

        direction = new Vector3(horizontal, 0, vertical).normalized;
        transform.rotation = Camera.main.transform.rotation;

        Debug.Log(Camera.main.transform.rotation);

        if (direction.magnitude >= 0.1f)
        {
            //controller.Move(direction * speed * Time.deltaTime);
            transform.Translate(direction * speed * Time.deltaTime);
        }
        
    }
}
