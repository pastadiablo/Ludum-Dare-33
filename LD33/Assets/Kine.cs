using UnityEngine;
using System.Collections;
using Pathfinding;

public class Kine : MonoBehaviour {

	protected static float PANIC_FACTOR = 30.0f;
	
	protected float BaseSpeed;
	protected float BaseTurningSpeed;
	public MineBotAI AI { get; protected set; }

	public delegate void TargetReachedAction(Kine kine);
	public event TargetReachedAction OnTargetReached;

	[SerializeField]
	protected bool _isPanicked;
	[HideInInspector]
	public bool IsPanicked {
		get { return _isPanicked; }
		set {
			this.AI.speed = (value ? PANIC_FACTOR * BaseSpeed : BaseSpeed);
			this.AI.turningSpeed = (value ? BaseTurningSpeed / PANIC_FACTOR * 10 : BaseTurningSpeed);
			_isPanicked = value;

			if(_isPanicked) {
				OnTargetReached(this); // Requests new random destination
			}
		}
	}

	[SerializeField]
	protected bool _isDead;
	[HideInInspector]
	public bool IsDead {
		get { return _isDead; }
		set { 
			this.AI.speed = (value ? 0 : BaseSpeed);
			_isDead = value;
		}
	}

	[SerializeField]
	protected int _blood;
	public int Blood {
		get { return _blood; }
		set {
			_blood = Mathf.Max(value, 0);

			if(_blood == 0) {
				this.IsDead = true;
			}
		}
	}

	void Awake () {
		this.AI = GetComponent<MineBotAI>();
		BaseSpeed = this.AI.speed;
		BaseTurningSpeed = this.AI.speed;
	}

	// Use this for initialization
	void Start () {
		this.AI.OnTargetReachedEvent += TargetReached;
		IsPanicked = _isPanicked;
		Blood = _blood;
		IsDead = _isDead;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void TargetReached() {
		OnTargetReached(this);

		if (this.IsPanicked) {
			this.IsPanicked = false;
		}
	}

	void OnTriggerEnter ( Collider collider){
		if (collider.gameObject == this.gameObject)
			return;

		Kine otherKine = collider.gameObject.GetComponent<Kine> ();
		if(otherKine && otherKine.gameObject.layer == 9) { // Humanoids
			// Freak out!
			if(otherKine.IsDead) {
				Debug.LogWarning("PANIC: " + this.gameObject.name + " saw " + otherKine.gameObject.name);
				this.IsPanicked = true;
			}
		}

		Vampire player = collider.gameObject.GetComponent<Vampire> ();
		if(player && player.gameObject.layer == 10) { // Player
			// Freak out!
			if(player.currentState == Vampire.VampireState.WALKING) {
				Debug.LogWarning("PANIC: " + this.gameObject.name + " saw the player running!");
				this.IsPanicked = true;
			}
		}
	}
}
