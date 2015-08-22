using UnityEngine;
using System.Collections;
using Pathfinding;

public class Kine : MonoBehaviour {

	public MineBotAI AI;
	public delegate void TargetReachedAction(Kine kine);
	public event TargetReachedAction OnTargetReached;

	void Awake () {
		this.AI = GetComponent<MineBotAI>();
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
	}
}
