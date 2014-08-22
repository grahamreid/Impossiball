using UnityEngine;
using System.Collections;

public class ControlOrientation : MonoBehaviour {
	
    public GameObject cameraLeft;
    public GameObject cameraRight;
	
	void Update()
	{
        transform.localEulerAngles = new Vector3(0,cameraLeft.transform.localEulerAngles.y,0);
		this.transform.position = GameObject.Find ("Sphere").transform.position;
	}

}
