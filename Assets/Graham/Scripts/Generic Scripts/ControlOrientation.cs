using UnityEngine;
using System.Collections;

public class ControlOrientation : MonoBehaviour {

	private float sensitivity = .25f;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = 0F;
	public float maximumY = 0F;

	float rotationY = 0F;
	float rotationX = 0F;
	// Use this for initialization

	private float rotateSpeed = 2f;
	void Start () {
		minimumY = transform.localEulerAngles.x - 40;
		maximumY = transform.localEulerAngles.x + 40;
	}
	
	void Awake () {
		
	}
	
	void Update()
	{
		if (Mathf.Abs (Input.GetAxis ("Pivot")) > sensitivity) {
						rotationX = transform.localEulerAngles.y + Input.GetAxis ("Pivot") * rotateSpeed;
				}
		if (Mathf.Abs(Input.GetAxis("Look")) > sensitivity)
			rotationY += Input.GetAxis("Look") * rotateSpeed;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		this.transform.position = GameObject.Find ("Sphere").transform.position;
	}

}
