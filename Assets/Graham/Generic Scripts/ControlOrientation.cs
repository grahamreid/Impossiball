using UnityEngine;
using System.Collections;

public class ControlOrientation : MonoBehaviour {

	private float sensitivity = .25f;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
    public GameObject cameraLeft;
    public GameObject cameraRight;
	
	public float minimumY = 0F;
	public float maximumY = 0F;

	float rotationY = 0F;
	float rotationX = 0F;
	// Use this for initialization

	private float rotateSpeed = 1.5f;
	void Start () {
		minimumY = transform.localEulerAngles.x - 80;
		maximumY = transform.localEulerAngles.x + 80;
	}
	
	void Awake () {
		
	}
	
	void Update()
	{
		/*if (Mathf.Abs (Input.GetAxis ("Pivot")) > sensitivity) {
						rotationX = transform.localEulerAngles.y + Input.GetAxis ("Pivot") * rotateSpeed;
				}
        if (Mathf.Abs(Input.GetAxis("Look")) > sensitivity)
        {
            rotationY += Input.GetAxis("Look") * rotateSpeed;
        }
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);*/

        transform.localEulerAngles = new Vector3(0,cameraLeft.transform.localEulerAngles.y,0);
        //cameraLeft.transform.Rotate(0, rotationY, 0);
        //cameraRight.transform.Rotate(0, rotationY, 0); 
		this.transform.position = GameObject.Find ("Sphere").transform.position;
	}

}
