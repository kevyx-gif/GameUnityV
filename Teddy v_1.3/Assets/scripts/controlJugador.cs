using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlJugador : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody rigidbody;
    private float jumpForce = 5;
    private bool injump;

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
        animator.SetBool("infloor", true);
        float movementspeed = 0;

        if(Input.GetKeyDown(KeyCode.Space)){
            injump = true;
            animator.SetTrigger("jump");
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

        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;

        animator.SetFloat("Speed", movementspeed );
    }


    private void FixedUpdate(){
        if(injump){
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            injump = false;
            animator.ResetTrigger("jump");
            animator.SetBool("infloor",false);
        }
    }


}
