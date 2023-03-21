using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlJugador : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody rigidbody;
    private float jumpForce = 4;
    private bool inAttack;
    private CapsuleCollider m_boxCollider;
    private bool isGrounded; // Boolean para ser seguro que tocamos el suelo

    public new Transform camera;
    public float speed = 2;
    // Start is called before the first frame update


    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision){

        isGrounded = false;
    }

    void Start()
    {
        m_boxCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 velocity = Vector3.zero;
        float movementspeed = 0;

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            animator.SetTrigger("jump");
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)){
            inAttack = true;
            animator.SetTrigger("attack");
        }

        if(Input.GetKeyDown("c")){
            speed = 4;
        }

        else if(Input.GetKeyUp("c")){
            speed = 2;
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

        if(isGrounded == false){
            animator.SetBool("infloor",false);
        }

        if(isGrounded == true){
            animator.SetBool("infloor",true);
            animator.ResetTrigger("jump");
        }


        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;

        if(rigidbody.velocity.x != 0 || rigidbody.velocity.z !=0){
            animator.SetBool("walk", true );
        }
        else{
            animator.SetBool("walk", false );
        }
        
    }


    private void FixedUpdate(){
        if(inAttack){
            inAttack = false;
            animator.ResetTrigger("attack");
        }
    }


}
