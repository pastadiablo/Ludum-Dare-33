using UnityEngine;
using System.Collections;

public class CameraLocker : MonoBehaviour {

	Quaternion rotation;

	// Use this for initialization
	void Awake () {
		rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.rotation = rotation;
	}
}
