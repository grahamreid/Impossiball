using UnityEngine;
using System.Collections;

public class ControlBall : MonoBehaviour {

	public GameObject LevelTransitionCollider;

	float floatSpeed = 10.0f;
	float floatRotationSpeed = 10.0f;
	float floatPivotSpeed = 1.5f;
	float floatMaxAngularVelocity = 5;
	Vector3 Vector3LastAngularVelocity;
	GameObject objOrientation;
	
	// Use this for initialization
	void Start () {
		objOrientation = GameObject.Find ("Orientation");
		if (Application.loadedLevel == 1)
				this.rigidbody.AddForce (0, -25, 0);
	}
	
	// Update is called once per frame
	void Update () {
		float floatRotationX = Input.GetAxis ("Forward")*floatRotationSpeed;
		float floatRotationZ = Input.GetAxis ("Strafe")*floatRotationSpeed;
		float floatRotationY = Input.GetAxis ("Pivot")*floatPivotSpeed;
		Vector3 v3ForwardRotation = objOrientation.transform.TransformDirection (Vector3.forward)*-floatRotationX;
		Vector3 v3ForwardCounterRotation = -1.0f * v3ForwardRotation;
		Vector3 v3SideRotation = objOrientation.transform.TransformDirection(Vector3.right)*-floatRotationZ;
		Vector3 v3SideCounterRotation = -1.0f * v3SideRotation;
		Vector3 v3PivotRotation = new Vector3(0,floatRotationY,0);
		if (floatRotationX < -floatRotationSpeed / 4 || floatRotationX > floatRotationSpeed / 4) {
			this.rigidbody.AddTorque (v3ForwardRotation);
				}
		if(floatRotationZ < -floatRotationSpeed/4 || floatRotationZ > floatRotationSpeed/4){
			this.rigidbody.AddTorque (v3SideRotation);

		}
		//if(floatRotationY < -floatPivotSpeed/4 || floatRotationY > floatPivotSpeed/4)
			//this.rigidbody.AddTorque (v3PivotRotation);
	}

	void LateUpdate() {
		if(this.rigidbody.angularVelocity.magnitude > floatMaxAngularVelocity)
			this.rigidbody.AddTorque(this.rigidbody.angularVelocity.x*(-1),0,this.rigidbody.angularVelocity.z*(-2));
		}


	void OnTriggerEnter(Collider other)
	{
	if(other.gameObject == LevelTransitionCollider)
			Application.LoadLevel("LevelTwo");
	}

}
