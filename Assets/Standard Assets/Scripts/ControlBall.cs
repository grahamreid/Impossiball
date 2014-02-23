using UnityEngine;
using System.Collections;

public class ControlBall : MonoBehaviour {
	
	float floatSpeed = 10.0f;
	float floatRotationSpeed = 5.0f;
	float floatPivotSpeed = 1f;
	Vector3 floatLastAngularVelocity;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float floatRotationX = Input.GetAxis ("Forward")*floatRotationSpeed;
		float floatRotationZ = Input.GetAxis ("Strafe")*floatRotationSpeed;
		float floatRotationY = Input.GetAxis ("Pivot")*floatPivotSpeed;
		Vector3 v3LeftRotationX = GameObject.Find ("Orientation").transform.TransformDirection (Vector3.forward)*floatRotationX;
		Vector3 v3LeftRotationZ = GameObject.Find("Orientation").transform.TransformDirection(Vector3.right)*floatRotationZ;
		Vector3 v3LeftRotationY = new Vector3(0,floatRotationY,0);
		if (floatRotationX < -floatRotationSpeed / 4 || floatRotationX > floatRotationSpeed / 4) 
			this.rigidbody.AddTorque (v3LeftRotationX);
		if(floatRotationZ < -floatRotationSpeed/4 || floatRotationZ > floatRotationSpeed/4)
			this.rigidbody.AddTorque (v3LeftRotationZ);
		if(floatRotationY < -floatPivotSpeed/4 || floatRotationY > floatPivotSpeed/4)
			this.rigidbody.AddTorque (v3LeftRotationY);
	}
}
