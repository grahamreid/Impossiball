using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	float floatPivotSpeed = .1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake () {
		
	}

	void LateUpdate()
	{
		this.transform.position = GameObject.Find ("Sphere").transform.position;
		print (GameObject.Find ("Sphere").rigidbody.angularVelocity.y);
		this.transform.Rotate(0,GameObject.Find("Sphere").rigidbody.angularVelocity.y,0);
	}
}
