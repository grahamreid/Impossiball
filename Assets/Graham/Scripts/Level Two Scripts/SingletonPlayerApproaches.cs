using UnityEngine;
using System.Collections;

public class SingletonPlayerApproaches : MonoBehaviour {

	private GameObject _player;
	private GameObject _platform;
	public float fltDistanceThreshold = 10;
	// Use this for initialization
	void Start () {
		_player = GameObject.Find ("Sphere").gameObject;
		_platform = this.transform.parent.transform.FindChild ("Platform").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 distanceThreshold = new Vector3 (this.transform.position.x - _player.transform.position.x,
		                                          0,
		                                         this.transform.position.z - _player.transform.position.z);
		if (distanceThreshold.magnitude < fltDistanceThreshold)
			_platform.GetComponent<FSMMoveGenericPlatform> ().EnterState_MovingSelf (FSMMoveGenericPlatform.MovingState.BeginForward);
		else
			_platform.GetComponent<FSMMoveGenericPlatform> ().EnterState_MovingSelf (FSMMoveGenericPlatform.MovingState.BeginReverse);
	}
}
