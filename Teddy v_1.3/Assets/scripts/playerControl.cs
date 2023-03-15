using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;


    public new Transform camera;
    public float speed = 4;
    public float gravity = -9.8f;
    public float jumpforce;
    // Start is called before the first frame update
    void Start()
    {
        //Una forma
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if(characterController.isGrounded){
            animator.SetBool("infloor", true);
        }

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;
        float movementspeed = 0;


        if (hor != 0 || ver != 0){
            Vector3 forward = camera.forward;
            forward.y=0;
            forward.Normalize();

            Vector3 right = camera.right;
            right.y = 0;
            right.Normalize();

            Vector3 direction = forward * ver + right * hor;
            movementspeed = Mathf.Clamp01(direction.magnitude);
            direction.Normalize();

            movement = direction * speed * movementspeed *Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction),0.2f);
        }

        movement.y += gravity + Time.deltaTime;

        if (characterController.isGrounded && Input.GetButtonDown("Jump")){
            movement.y = -jumpforce + Time.deltaTime;
            animator.SetBool("jump",true);
            animator.SetBool("infloor",false);
        }

        else if(!characterController.isGrounded){
            animator.SetBool("infloor",false);
        }

        characterController.Move(movement);
        animator.SetFloat("Speed", movementspeed );
    }

}
