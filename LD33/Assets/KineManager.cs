using UnityEngine;
using System.Collections;

public class KineManager : MonoBehaviour {

	public int NumberOfKines;
	protected IList Kines;

	void Awake () {
		this.Kines = FindObjectsOfType(typeof(Kine));
	}

	// Use this for initialization
	void Start () {
		foreach (Kine kine in Kines) {
			GameObject kineTarget = new GameObject(kine.name + " TARGET");
			kineTarget.transform.parent = this.transform;
			kineTarget.transform.localPosition = new Vector3(Random.value * 49, 0, Random.value * 49);

			kine.AI.target = kineTarget.transform;

			kine.OnTargetReached += KineReachedTarget;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void KineReachedTarget(Kine kine){
		kine.AI.target.localPosition = new Vector3 (Random.value * 49, 0, Random.value * 49);
	}
}
