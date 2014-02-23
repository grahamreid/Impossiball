using UnityEngine;
using System.Collections;

public class ControlOrientation : MonoBehaviour {
	float floatPivotSpeed = .1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = GameObject.Find ("Sphere").transform.position;
		this.transform.Rotate(0,GameObject.Find("Sphere").rigidbody.angularVelocity.y,0);
	}
}
