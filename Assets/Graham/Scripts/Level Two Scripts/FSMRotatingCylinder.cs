using UnityEngine;
using System.Collections;

public class FSMRotatingCylinder : MonoBehaviour {

	public int intRotateSpeed;
	public bool _blnColliderOnly;
	private bool _blnPlayerOnPlatform;
	private GameObject _player;

	private const float pi = 3.14f;
	// Use this for initialization
	void Start () {
		_blnPlayerOnPlatform = false;
		if (_blnColliderOnly) {
			intRotateSpeed = this.transform.parent.GetComponent<FSMRotatingCylinder> ().intRotateSpeed;
		}
		_player = GameObject.Find ("Player").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(_blnColliderOnly == false)
			transform.Rotate (0,intRotateSpeed*Time.deltaTime,0);	
		if (_blnPlayerOnPlatform) {
			_player.transform.RotateAround (this.transform.position, this.transform.up,intRotateSpeed*Time.deltaTime );
				}
	}

	void OnTriggerEnter(Collider other)
	{
		switch(other.name){
			case("Sphere"):
			_blnPlayerOnPlatform = true;
			break;
		}
	}
	void OnTriggerExit(Collider other)
	{
		switch(other.name){
		case("Sphere"):
			_blnPlayerOnPlatform = false;
			break;
		}
	}
}
