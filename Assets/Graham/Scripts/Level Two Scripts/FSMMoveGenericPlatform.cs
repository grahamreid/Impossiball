using UnityEngine;
using System.Collections;

public class FSMMoveGenericPlatform : MonoBehaviour {
	
	public GameObject[] checkpoints;
	public GameObject[] wallsToMoveStart;
	public GameObject[] wallsToMoveEnd;
	
	public  int intSpeed;
	public GameObject objCheckpoint;

	private const string strPlayerName = "Sphere";
	private const float fltDistanceThreshold = .075f;
	private GameObject currentDestination;
	private bool _blnPlayerOnPlatform;
	private int _intDestinationIndex;
	private GameObject[] currentWallSetToMove;
	private bool _blnMovingForward;

	private GameObject _player;


	private enum WallSet{
		startingWallSet,
		endingWallSet
	}
	private WallSet _currentMovingWalls;
	public enum States{
		Waiting,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
	}
	public States currentState;
	// Use this for initialization
	void Start () {
		States currentState = States.Waiting;	
		this.collider.enabled = true;
		_player = GameObject.Find ("Player").gameObject;
	}
	
	void Update () {
		switch (currentState) {
		case(States.Waiting):

		case(States.RaisingWalls):
			foreach(GameObject wall in currentWallSetToMove)
				if(wall.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Raised)
					if(_currentMovingWalls == WallSet.startingWallSet)
						EnterState_MovingSelf(1,true,true);
					else
						EnterState_MovingSelf(checkpoints.Length-2,false,false);
			break;
		case(States.LoweringWalls):
			foreach(GameObject wall in currentWallSetToMove)
				if(wall.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Lowered)
					currentState = States.Waiting;
			break;
		case(States.MovingSelf):
			this.transform.position += intSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if(_blnPlayerOnPlatform)
				_player.transform.position += intSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if((this.transform.position - currentDestination.transform.position).magnitude < fltDistanceThreshold)
				if(currentDestination == checkpoints[checkpoints.Length-1])
					EnterState_LoweringWalls(WallSet.endingWallSet);
				else if(currentDestination == checkpoints[0])
					EnterState_LoweringWalls(WallSet.startingWallSet);
				else
					if(_blnMovingForward)
						EnterState_MovingSelf(_intDestinationIndex+1,_blnPlayerOnPlatform,true);
					else
						EnterState_MovingSelf(_intDestinationIndex-1,_blnPlayerOnPlatform,false);
			break;
		}
	}
	
	private void EnterState_RaisingWalls(WallSet wallSetToMove){
		this.collider.enabled = false;
		if (wallSetToMove == WallSet.startingWallSet)
			currentWallSetToMove = wallsToMoveStart;
		else
			currentWallSetToMove = wallsToMoveEnd;
		_currentMovingWalls = wallSetToMove;
		foreach (GameObject wall in currentWallSetToMove)
			wall.GetComponent<FSMMoveWall> ().EnterState_Raising ();
		currentState = States.RaisingWalls;
		
	}
	
	private void EnterState_LoweringWalls(WallSet wallSetToMove){
		this.collider.enabled = false;
		if (wallSetToMove == WallSet.startingWallSet)
			currentWallSetToMove = wallsToMoveStart;
		else
			currentWallSetToMove = wallsToMoveEnd;
		_currentMovingWalls = wallSetToMove;
		foreach (GameObject wall in currentWallSetToMove)
			wall.GetComponent<FSMMoveWall> ().EnterState_Lowering ();
		currentState = States.LoweringWalls;
		
	}
	
	public void EnterState_MovingSelf(int intDestinationIndex,bool blnPlayerOnPlatform, bool blnMovingForward){
		currentDestination = checkpoints[intDestinationIndex];
		_blnPlayerOnPlatform = blnPlayerOnPlatform;
		_intDestinationIndex = intDestinationIndex;
		_blnMovingForward = blnMovingForward;
		currentState = States.MovingSelf;
	}
	
	public void OnTriggerEnter(Collider other)
	{
		switch (currentState) {
		case(States.Waiting):
			EnterState_RaisingWalls(WallSet.startingWallSet);
			break;
			
		}
	}
}