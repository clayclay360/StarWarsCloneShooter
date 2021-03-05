using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControllerTest1 : MonoBehaviour
{
    [SerializeField]
    private float runSpeed, rotationSpeed;

    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,0, Input.GetAxis("Vertical")) * runSpeed * Time.deltaTime);
        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal")) * rotationSpeed * Time.deltaTime);
        playerAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }
}
