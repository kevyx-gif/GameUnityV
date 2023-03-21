using UnityEngine;
using System.Collections;

public class Action_Manage : MonoBehaviour {
	private Animator animator;
	
	public GameObject Target;
	public int rutina;
	public float cronometro;
	public Quaternion angulo;
	public float grado;
	float movementspeed = 0;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Comportamiento_Enemigo();
	}

	public void Comportamiento_Enemigo(){
		if (Vector3.Distance(transform.position, Target.transform.position) > 5){
			animator.SetBool("run",false);
			cronometro += 1 * Time.deltaTime;

			if (cronometro >= 2){
				rutina = Random.Range(0,2);
				cronometro = 0;
			}

			switch (rutina)
			{
				case 0:
					movementspeed = 0;
					animator.SetBool("walk",false);
					break;
				case 1:
					grado = Random.Range(0,360);
					angulo = Quaternion.Euler(0,grado,0);
					rutina++;
					break;
				case 2:
					movementspeed = 1;
					transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
					transform.Translate(Vector3.forward * movementspeed * Time.deltaTime);
					animator.SetBool("walk",true);
					break;

			}
		}
		else if(Vector3.Distance(transform.position, Target.transform.position) > 1.6 && Vector3.Distance(transform.position, Target.transform.position) < 5){
			movementspeed = 2;
			var lookPos = Target.transform.position - transform.position;
			lookPos.y = 0;
			var rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
			animator.SetBool("atackMoment",false);
			animator.SetBool("walk",false);
			animator.SetBool("run",true);
			transform.Translate(Vector3.forward * movementspeed * Time.deltaTime);
		}
		else{
			movementspeed = 0;
			animator.SetBool("walk",false);
			animator.SetBool("run",false);
			animator.SetBool("atackMoment",true);
		}
	}
}
