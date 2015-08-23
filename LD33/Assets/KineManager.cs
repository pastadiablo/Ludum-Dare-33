using UnityEngine;
using System.Collections;

public class KineManager : MonoBehaviour {
	public Transform NormalKinePrefab;
	public int NumberOfKines;
	protected IList Kines;

	void Awake () {
		this.Kines = FindObjectsOfType(typeof(Kine));
	}

	// Use this for initialization
	void Start () {
		// Create new Kines to hit the editor-specified max #
		/*for (int k=this.Kines.Count; k<=NumberOfKines; k++) {
			GameObject kine = Instantiate(NormalKinePrefab, RandomGridLocation(), Quaternion.identity) as GameObject;
			kine.name = "Kine " + k;
		}*/

		// Set them all up and kick their routines off
		foreach (Kine kine in Kines) {
			GameObject kineTarget = new GameObject(kine.name + " TARGET");
			kineTarget.transform.parent = this.transform;
			kineTarget.transform.localPosition = RandomGridLocation();

			kine.AI.target = kineTarget.transform;

			kine.OnTargetReached += KineReachedTarget;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	Vector3 RandomGridLocation() {
		return new Vector3 (Random.value * 48 + 1, 0, Random.value * 48 + 1);
	}

	public void KineReachedTarget(Kine kine){
		kine.AI.target.localPosition = new Vector3 (Random.value * 49, 0, Random.value * 49);
	}
}
