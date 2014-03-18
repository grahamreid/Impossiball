using UnityEngine;
using System.Collections;

public class SingletonRecoverPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		this.transform.FindChild ("Platform").GetComponent<FSMMoveGenericPlatform> ().EnterState_RecoverPlayer ();
	}
}
