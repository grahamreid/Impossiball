using UnityEngine;
using System.Collections;

public class MoveElevator : MonoBehaviour {
	public  int intSpeed;
	public GameObject objDestination;
	private GameObject objWallToMove;
	private float fltReferenceHeight;
	private string strDestinationName;
	private const string strPlayerName = "Sphere";
	enum States{
		Waiting,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
		Finished
	}
	States currentState;
	// Use this for initialization
	void Start () {
		States currentState = States.Waiting;	
		strDestinationName = objDestination.name;
	}
	
	void Update () {
		switch (currentState) {
		case(States.Waiting):
			break;		
		case(States.RaisingWalls):
			objWallToMove.transform.position += new Vector3(0,1,0)*Time.deltaTime;
			if(objWallToMove.transform.position.y >= fltReferenceHeight+1)
				currentState = States.MovingSelf;
			break;
		case(States.MovingSelf):
			this.transform.position += intSpeed * (objDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if(objWallToMove.transform.position == objDestination.transform.position)
				currentState = States.Finished;
			break;
		case(States.Finished):
			break;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.name == strPlayerName)
		switch(currentState){
			case(States.Waiting):
			RaisingWallsEnterState("PlatformWall1");
			break;
		}
		if(other.name == strDestinationName)
		switch(currentState){
			case(States.MovingSelf):
			this.currentState = States.Finished;
			break;	
		}
	}
	
	void RaisingWallsEnterState(string strWallToMove){
		currentState = States.RaisingWalls;
		objWallToMove = this.transform.FindChild (strWallToMove).gameObject;
		fltReferenceHeight = objWallToMove.transform.position.y;
	}
}
