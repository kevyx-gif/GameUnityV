using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlJugador : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody rigidbody;
    private float jumpForce = 5;
    private bool injump;
    private bool inAttack;
    private bool canJump;

    public new Transform camera;
    public float speed = 4;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 velocity = Vector3.zero;
        float movementspeed = 0;
        canJump = true;

        if(Input.GetKeyDown(KeyCode.Space) && canJump == true){
            injump = true;
            animator.SetTrigger("jump");
            canJump = false;
            animator.SetBool("infloor",false);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)){
            inAttack = true;
            animator.SetTrigger("attack");
        }


        if (hor != 0 || ver != 0){
            //mov
            Vector3 forward = camera.forward;
            forward.y=0;
            forward.Normalize();

            Vector3 right = camera.right;
            right.y = 0;
            right.Normalize();

            Vector3 direction = forward * ver + right * hor;
            movementspeed = Mathf.Clamp01(direction.magnitude);
            direction.Normalize();

            velocity = direction * speed ;

            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction),0.2f);

            
        }

        if(injump && animator.GetBool("infloor") == false){
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            animator.ResetTrigger("jump");
            animator.SetBool("infloor",true);
            canJump = true;
        }


        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;

        animator.SetFloat("Speed", movementspeed );
    }


    private void FixedUpdate(){

        if(inAttack){
            inAttack = false;
            animator.ResetTrigger("attack");
        }
    }


}
