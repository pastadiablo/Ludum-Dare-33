using UnityEngine;
using System.Collections;

public class Vampire : MonoBehaviour {

	public enum VampireState {
		IDLE,
		WALKING,
		RUNNING,
		POUNCING
	};

	public Vector2 movement;

	public float walkSpeed = 1.0f;
	public float runSpeed = 2.0f;
	public float pounceSpeed = 3.0f;
	float moveSpeed;

	public VampireState currentState;

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(currentState != VampireState.POUNCING) {

			//If player holding down left-shift, set movement speed to run, otherwise, to walk
			if(Input.GetKey(KeyCode.LeftShift)) {
				moveSpeed = runSpeed;
			} else {
				moveSpeed = walkSpeed;
			}

			movement = new Vector2(0.0f, 0.0f);

			if(Input.GetKey(KeyCode.W)) { //UP 
				movement = new Vector2(movement.x, 1);
				currentState = VampireState.WALKING;
			}
			else if(Input.GetKey(KeyCode.S)) { //DOWN
				movement = new Vector2(movement.x, -1);
				currentState = VampireState.WALKING;
			}
			
			if(Input.GetKey(KeyCode.A)) { //LEFT
				movement = new Vector2(-1, movement.y);
				currentState = VampireState.WALKING;
			}
			else if(Input.GetKey(KeyCode.D)) { //RIGHT
				movement = new Vector2(1, movement.y);
				currentState = VampireState.WALKING;
			}
		
			if (movement.x == 0.0f && movement.y == 0.0f ) {
				currentState = VampireState.IDLE;
			} else if (Input.GetKey (KeyCode.LeftShift)) {
				currentState = VampireState.RUNNING;
			}
		}

		if(currentState != VampireState.IDLE) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				currentState = VampireState.POUNCING;
			}
		}

		switch(currentState) {

		case VampireState.IDLE:

			//TODO: remember last direction
			animator.SetBool("moving", false);
			break;

		case VampireState.WALKING:
		case VampireState.RUNNING:
			
			animator.SetBool("moving", true);

			break;
		case VampireState.POUNCING:

			animator.SetBool("pouncing", true);
			animator.SetBool("moving", false);
			moveSpeed = pounceSpeed;

			break;
		}


		animator.SetInteger("horizontal", (int)movement.x);
		animator.SetInteger("vertical", (int)movement.y);

		transform.localPosition = new Vector3(transform.localPosition.x + movement.x * Time.deltaTime * moveSpeed,
		                                      transform.localPosition.y + movement.y * Time.deltaTime * moveSpeed,
		                                      0);
	}

	public void EndPounce() {
		animator.SetBool("pouncing", false);
		currentState = VampireState.IDLE;
	}
}
