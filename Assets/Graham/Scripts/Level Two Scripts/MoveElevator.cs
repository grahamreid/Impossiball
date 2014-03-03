using UnityEngine;
using System.Collections;

public class MoveElevator : MonoBehaviour {
	public  int intSpeed;
	public GameObject objDestination;
	private GameObject objWallToMove;
	private float fltReferenceHeight;
	private string strDestinationName;
	private const string strPlayerName = "Sphere";
	enum State{
		Waiting,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
		Finished
	}
	State currentState;
	// Use this for initialization
	void Start () {
		State currentState = State.Waiting;	
		strDestinationName = objDestination.name;
	}
	
	void Update () {
		switch (currentState) {
		case(State.Waiting):
			break;		
		case(State.RaisingWalls):
			objWallToMove.transform.position += new Vector3(0,1,0)*Time.deltaTime;
			if(objWallToMove.transform.position.y >= fltReferenceHeight+1)
				currentState = State.MovingSelf;
			break;
		case(State.MovingSelf):
			this.transform.position += intSpeed * (objDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if(objWallToMove.transform.position == objDestination.transform.position)
				currentState = State.Finished;
			break;
		case(State.Finished):
			break;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.name == strPlayerName)
		switch(currentState){
			case(State.Waiting):
			RaisingWallsEnterState("PlatformWall1");
			break;
		}
		if(other.name == strDestinationName)
		switch(currentState){
			case(State.MovingSelf):
			this.currentState = State.Finished;
			break;	
		}
	}
	
	void RaisingWallsEnterState(string strWallToMove){
		currentState = State.RaisingWalls;
		objWallToMove = this.transform.FindChild (strWallToMove).gameObject;
		fltReferenceHeight = objWallToMove.transform.position.y;
	}
}
