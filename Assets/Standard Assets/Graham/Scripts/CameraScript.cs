using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}

	void Awake () {
		
	}

	void Update()
	{
		this.transform.position = GameObject.Find ("Orientation").transform.position;
		this.transform.rotation = GameObject.Find ("Orientation").transform.rotation;
	}
}
