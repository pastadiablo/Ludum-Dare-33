using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Vampire : MonoBehaviour {

	public enum VampireState {
		IDLE,
		WALKING,
	};

	public Vector2 movement;

	public float walkSpeed = 1.0f;
	public float runSpeed = 2.0f;
	float moveSpeed;

	public VampireState currentState;


	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
		//If player holding down left-shift, set movement speed to run, otherwise, to walk
		if(Input.GetKey(KeyCode.LeftShift)) {
			moveSpeed = runSpeed;
		} else {
			moveSpeed = walkSpeed;
		}
		
		movement = new Vector2(0.0f, 0.0f);

		if(Input.GetKey(KeyCode.W)) { //UP 
			movement = new Vector2(movement.x, moveSpeed);
			animator.Play("WalkUp");
			currentState = VampireState.WALKING;
		}
		else if(Input.GetKey(KeyCode.S)) { //DOWN
			movement = new Vector2(movement.x, -moveSpeed);
			animator.Play("WalkDown");
			currentState = VampireState.WALKING;
		}
		
		if(Input.GetKey(KeyCode.A)) { //LEFT
			movement = new Vector2(-moveSpeed, movement.y);
			animator.Play("WalkLeft");
			currentState = VampireState.WALKING;
		}
		else if(Input.GetKey(KeyCode.D)) { //RIGHT
			movement = new Vector2(moveSpeed, movement.y);
			animator.Play("WalkRight");
			currentState = VampireState.WALKING;
		}
	
		if(movement.x == 0.0f && movement.y == 0.0f)
			currentState = VampireState.IDLE;

		switch(currentState) {

		case VampireState.IDLE:

			//TODO: remember last direction
			animator.Play("IdleDown");
			break;

		case VampireState.WALKING:

			break;
		}

		transform.position = new Vector3(transform.position.x + movement.x * Time.deltaTime,
		                                 transform.position.y + movement.y * Time.deltaTime,
		                                 0);
	}
}
