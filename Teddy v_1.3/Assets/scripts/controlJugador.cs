using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlJugador : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody rigidbody;
    private float jumpForce = 6;
    private CapsuleCollider m_boxCollider;
    private bool isGrounded; // Boolean para ser seguro que tocamos el suelo
    private bool canjump = true;

    public float atackCooldown = 0.25f;
    float lastAtackTime = 0;

    public new Transform camera;
    public float speed = 2;
    // Start is called before the first frame update


    void OnCollisionEnter(Collision collision)
    {
        canjump = true;
        isGrounded = true;
        animator.SetBool("infloor",true);
        animator.ResetTrigger("jump");
    }

    void OnCollisionExit(Collision collision){
        canjump = false;
        isGrounded = false;
        animator.SetBool("infloor",false);
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
        //Debug.Log("combos " + combo.ToString() + " puede combear "+canCombo + " puede atacar "+ canAttack + " in floor " + animator.GetBool("infloor"));
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 velocity = Vector3.zero;
        float movementspeed = 0;


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

        if(rigidbody.velocity.x != 0 || rigidbody.velocity.z !=0){
            animator.SetBool("walk", true );
        }
        else{
            animator.SetBool("walk", false );
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canjump){
            animator.SetTrigger("jump");
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)){
            atack();
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("AttackCombo03")){
            atackCooldown = 2f;
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")){
            atackCooldown = 1f;
            animator.ResetTrigger("attack");
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Battle")){
            atackCooldown = 0.25f;
        }

        if(Input.GetKeyDown("c")){
            animator.SetBool("isrun",true);
            speed = 4;
        }

        else if(Input.GetKeyUp("c")){
            animator.SetBool("isrun",false);
            speed = 2;
        }
        
    }

    void atack(){
        if(Time.time - lastAtackTime > atackCooldown){
            animator.SetTrigger("attack");
            lastAtackTime = Time.time;
        }
        else{
            animator.ResetTrigger("attack");
        }
    }

}
