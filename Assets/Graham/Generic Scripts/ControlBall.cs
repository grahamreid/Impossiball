using UnityEngine;
using System.Collections;

public class ControlBall : MonoBehaviour {

	public GameObject LevelTransitionCollider;

	//physics variables
	float floatSpeedFactor = 3.0f;
	float floatPivotSpeed = 1.5f;
	float floatMaxAngularVelocity = 5;
	float floatCounterAngularForce = -0.75f;

	//object handles
	public GameObject objOrientation;
    public GameObject objCamera;

	//for level transitions
	bool blnBeginFadeCamera;
	float alphaFadeValue;
	private Texture2D txtBlackTexture;
	
	// Use this for initialization
	void Start () {

		//instantiate handles
		objOrientation = GameObject.Find ("Orientation");
        objCamera = GameObject.Find("CameraLeft");

		//at beginning of second level player is already falling
		if (Application.loadedLevel == 1)
				this.rigidbody.AddForce (0, -25, 0);

		//invisible texture in front of the camera. 
		//This will turn black once the player has reached the end of the level.
		blnBeginFadeCamera = false;
		alphaFadeValue = 0;
		txtBlackTexture = new Texture2D (1280, 720, TextureFormat.ARGB32, false);
		for(int i = 0;i<1280;i++)
		    for(int j = 0;j<720;j++)
				txtBlackTexture.SetPixel(i,j,Color.black);

		txtBlackTexture.Apply ();

	}
	
	// Update is called once per frame
	void Update () {

		//magnitudes of the torque vectors to be applied
        float floatRotationX;
        float floatRotationZ;

		//get the offset of the camera's rotation from the normal x and z planes, respectively
        floatRotationX = 	(objCamera.transform.localEulerAngles.x > 180) 	?
							objCamera.transform.localEulerAngles.x-360     	:
							objCamera.transform.localEulerAngles.x;
		floatRotationZ = 	(objCamera.transform.localEulerAngles.z > 180) 	?
							objCamera.transform.localEulerAngles.z-360		:
							objCamera.transform.localEulerAngles.z;

		//Slow the angular speed down a bit
		floatRotationX /= floatSpeedFactor;
		floatRotationZ /= floatSpeedFactor;

		//Apply scalar values to the orientation's forward and side vectors 
		//to get vectors with magnitudes that we can use for torque
		Vector3 v3ForwardRotation = objOrientation.transform.TransformDirection (Vector3.forward)*floatRotationZ;
        Vector3 v3SideRotation = objOrientation.transform.TransformDirection(Vector3.right)*floatRotationX;

		//apply the torques
		this.rigidbody.AddTorque (v3SideRotation);
		this.rigidbody.AddTorque (v3ForwardRotation);

		if (blnBeginFadeCamera)
			alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 5);
		if (alphaFadeValue >= 1) 
			Application.Quit ();
				

	}

	//apply counter-torques to keep the player from going too fast
	void LateUpdate() {
		if(this.rigidbody.angularVelocity.magnitude > floatMaxAngularVelocity)
			this.rigidbody.AddTorque(this.rigidbody.angularVelocity.x*(floatCounterAngularForce),
			                         0,this.rigidbody.angularVelocity.z*(floatCounterAngularForce));
	}

	void OnGUI()
	{
		if (blnBeginFadeCamera) {
						GUI.color = new Color (0, 0, 0, alphaFadeValue);
						GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), txtBlackTexture);
				}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == LevelTransitionCollider)
				if (Application.loadedLevelName == "LevelOne")
						Application.LoadLevel ("LevelTwo");
				else if (Application.loadedLevelName == "LevelTwo") {
					blnBeginFadeCamera = true;
				}
	}

}
