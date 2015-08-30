﻿using UnityEngine;
using System.Collections;

public class Vampire : MonoBehaviour {

	public enum VampireState {
		IDLE,
		WALKING,
		RUNNING,
		POUNCING,
		FEEDING
	};

	public Vector2 movement;

	public float walkSpeed = 1.0f;
	public float runSpeed = 2.0f;
	public float pounceSpeed = 3.0f;
	float moveSpeed;

	protected float _blood;
	public float Blood {
		get { return _blood; }
		set { 
			_blood = Mathf.Max(0,Mathf.Min (100, value));
		}
	}

	protected float DRAIN_SPEED = 1.0f;
	public VampireState currentState;

	protected Kine sittingOnAKine = null;
	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update () {
		if (currentState == VampireState.FEEDING && sittingOnAKine) {
			sittingOnAKine.IsDead = true;
			sittingOnAKine.Blood -= (int)(Time.deltaTime * DRAIN_SPEED);
			this.Blood += (int)(Time.deltaTime * DRAIN_SPEED);
			if(sittingOnAKine.Blood <= 0 || sittingOnAKine.IsDead) {
				this.EndFeeding();
			}
		}

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
			animator.SetBool("feeding", false);

			break;
		case VampireState.POUNCING:

			animator.SetBool("pouncing", true);
			animator.SetBool("moving", false);
			moveSpeed = pounceSpeed;

			break;
		case VampireState.FEEDING:
			animator.SetBool("feeding", true);

			break;
		}


		animator.SetInteger("horizontal", (int)movement.x);
		animator.SetInteger("vertical", (int)movement.y);

		movement.Normalize();
		movement *= moveSpeed * Time.deltaTime;

		this.GetComponent<CharacterController>().Move(new Vector3(movement.x,
		                                                          0,
		                                                          movement.y));

		// currentState = VampireState.FEEDING;
	}

	public void EndPounce() {
		animator.SetBool("pouncing", false);
		currentState = VampireState.IDLE;
		
		if(sittingOnAKine) {
			currentState = VampireState.FEEDING;
			animator.SetBool("feeding", true);
		}
	}

	public void EndFeeding() {
		animator.SetBool("feeding", false);
		sittingOnAKine = null;
		currentState = VampireState.IDLE;
	}

	void OnTriggerEnter ( Collider collider){
		if (collider.gameObject == this.gameObject)
			return;

		// Make note of Kine the player sits upon
		Kine otherKine = collider.gameObject.GetComponent<Kine> ();
		if(otherKine && otherKine.gameObject.layer == 9) { // Humanoids
			sittingOnAKine = otherKine;
		}
	}

	void OnTriggerExit ( Collider collider){
		if (collider.gameObject == this.gameObject)
			return;
		
		Kine otherKine = collider.gameObject.GetComponent<Kine> ();
		if(otherKine && otherKine.gameObject.layer == 9 && otherKine == sittingOnAKine) { // Humanoids
			sittingOnAKine = null;
		}
	}
}
