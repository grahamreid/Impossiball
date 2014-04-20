using UnityEngine;
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
	bool blnBeginFadeCamera;
	float alphaFadeValue;
	private Texture2D txtBlackTexture;
	
	// Use this for initialization
	void Start () {
		objOrientation = GameObject.Find ("Orientation");
        objCamera = GameObject.Find("CameraLeft");
		if (Application.loadedLevel == 1)
				this.rigidbody.AddForce (0, -25, 0);
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
		floatRotationX /= 3;
		floatRotationZ /= 3;
		float floatRotationY = Input.GetAxis ("Pivot")*floatPivotSpeed;
		Vector3 v3ForwardRotation = objOrientation.transform.TransformDirection (Vector3.forward)*floatRotationZ;
		Vector3 v3ForwardCounterRotation = -1.0f * v3ForwardRotation;
        Vector3 v3SideRotation = objOrientation.transform.TransformDirection(Vector3.right)*floatRotationX;
		Vector3 v3SideCounterRotation = -1.0f * v3SideRotation;
		Vector3 v3PivotRotation = new Vector3(0,floatRotationY,0);
		this.rigidbody.AddTorque (v3SideRotation);
		this.rigidbody.AddTorque (v3ForwardRotation);
		if (blnBeginFadeCamera)
			alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 5);
		if (alphaFadeValue >= 1) 
			Application.Quit ();
				

	}

	void LateUpdate() {
		if(this.rigidbody.angularVelocity.magnitude > floatMaxAngularVelocity)
			this.rigidbody.AddTorque(this.rigidbody.angularVelocity.x*(-.75f),0,this.rigidbody.angularVelocity.z*(-.75f));
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
