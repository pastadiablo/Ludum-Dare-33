using UnityEngine;
using System.Collections;

public class AnimationFunctionRelayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EndPounce() {
		GetComponentInParent<Vampire>().EndPounce();
	}
}
