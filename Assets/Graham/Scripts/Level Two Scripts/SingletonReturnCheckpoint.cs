using UnityEngine;
using System.Collections;

public class SingletonReturnCheckpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider other)
	{
		transform.parent.transform.FindChild ("Platform").GetComponent<FSMMoveGenericPlatform> ().EnterState_RaisingWalls(FSMMoveGenericPlatform.WallSet.endingWallSet);
	}
}
