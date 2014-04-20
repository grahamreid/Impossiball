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
		if(transform.parent.transform.FindChild ("Platform").GetComponent<FSMMoveGenericPlatform> ().wallsToMoveEnd[0].gameObject.GetComponent<FSMMoveWall>().currentState != FSMMoveWall.State.Raised)
			transform.parent.transform.FindChild ("Platform").GetComponent<FSMMoveGenericPlatform> ().EnterState_RaisingWalls(FSMMoveGenericPlatform.WallSet.endingWallSet);
	}
}
