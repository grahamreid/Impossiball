﻿using UnityEngine;
using System.Collections;

public class ControlBall : MonoBehaviour {

	public GameObject LevelTransitionCollider;

	float floatSpeed = 10.0f;
	float floatRotationSpeed = 7.5f;
	float floatPivotSpeed = 1.5f;
	float floatMaxAngularVelocity = 5;
	Vector3 Vector3LastAngularVelocity;
	GameObject objOrientation;
    GameObject objCamera;
	
	// Use this for initialization
	void Start () {
		objOrientation = GameObject.Find ("Orientation");
        objCamera = GameObject.Find("CameraLeft");
		if (Application.loadedLevel == 1)
				this.rigidbody.AddForce (0, -25, 0);
	}
	
	// Update is called once per frame
	void Update () {
        float floatRotationX;
        float floatRotationZ;
        if (objCamera.transform.localEulerAngles.x > 180)
            floatRotationX = objCamera.transform.localEulerAngles.x-360;
        else
            floatRotationX = objCamera.transform.localEulerAngles.x;
        if (objCamera.transform.localEulerAngles.z > 180)
            floatRotationZ = objCamera.transform.localEulerAngles.z - 360;
        else
            floatRotationZ = objCamera.transform.localEulerAngles.z;
		float floatRotationY = Input.GetAxis ("Pivot")*floatPivotSpeed;
		Vector3 v3ForwardRotation = Vector3.Project(objCamera.transform.localEulerAngles,objOrientation.transform.TransformDirection (Vector3.forward));
		Vector3 v3ForwardCounterRotation = -1.0f * v3ForwardRotation;
        Vector3 v3SideRotation = Vector3.Project(objCamera.transform.localEulerAngles, objOrientation.transform.TransformDirection(Vector3.right));
		Vector3 v3SideCounterRotation = -1.0f * v3SideRotation;
		Vector3 v3PivotRotation = new Vector3(0,floatRotationY,0);
			this.rigidbody.AddTorque (floatRotationX,0,floatRotationZ);

		//if(floatRotationY < -floatPivotSpeed/4 || floatRotationY > floatPivotSpeed/4)
			//this.rigidbody.AddTorque (v3PivotRotation);
	}

	void LateUpdate() {
		if(this.rigidbody.angularVelocity.magnitude > floatMaxAngularVelocity)
			this.rigidbody.AddTorque(this.rigidbody.angularVelocity.x*(-.75f),0,this.rigidbody.angularVelocity.z*(-.75f));
		}


	void OnTriggerEnter(Collider other)
	{
	if(other.gameObject == LevelTransitionCollider)
			Application.LoadLevel("LevelTwo");
	}

}
