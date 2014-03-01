using UnityEngine;
using System.Collections;

public class ControlOrientation : MonoBehaviour {
	float floatPivotSpeed = 2f;
	// Use this for initialization
	void Start () {
		
	}
	
	void Awake () {
		
	}
	
	void Update()
	{
		float floatRotationY = Input.GetAxis ("Pivot")*floatPivotSpeed;
		if(floatRotationY < -floatPivotSpeed/4 || floatRotationY > floatPivotSpeed/4)
			this.transform.Rotate(0,floatRotationY,0);
		this.transform.position = GameObject.Find ("Sphere").transform.position;
	}

}
