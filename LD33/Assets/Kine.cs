using UnityEngine;
using System.Collections;
using Pathfinding;

public class Kine : MonoBehaviour {

	protected static float PANIC_FACTOR = 2.0f;
	
	protected float BaseSpeed;
	protected float BaseTurningSpeed;
	public MineBotAI AI { get; protected set; }

	public delegate void TargetReachedAction(Kine kine);
	public event TargetReachedAction OnTargetReached;

	[SerializeField]
	protected bool _isPanicked;
	public bool IsPanicked {
		get { return _isPanicked; }
		set {
			this.AI.speed = (value ? PANIC_FACTOR * BaseSpeed : BaseSpeed);
			this.AI.turningSpeed = (value ? BaseTurningSpeed / PANIC_FACTOR : BaseTurningSpeed);
			_isPanicked = value;
		}
	}

	[SerializeField]
	protected bool _isDead;
	public bool IsDead {
		get { return _isDead; }
		set { _isDead = value; }
	}

	public int Blood;

	void Awake () {
		this.AI = GetComponent<MineBotAI>();
		BaseSpeed = this.AI.speed;
		BaseTurningSpeed = this.AI.speed;

		_isPanicked = false;
		_isDead = false;

	}

	// Use this for initialization
	void Start () {
		this.AI.OnTargetReachedEvent += TargetReached;
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
}
