using UnityEngine;
using System.Collections;

public class FSMMoveGenericPlatform : MonoBehaviour {
	
	public GameObject[] checkpoints;
	public GameObject[] wallsToMoveStart;
	public GameObject[] wallsToMoveEnd;
	public GameObject returnCheckpoint;
	
	public  float fltSpeed;
	public bool blnTimedPlatform;
	public int intTicksMoving;
	public int intTicksWaiting;
	public int intSecondsPerTick;

	private const string strPlayerName = "Sphere";
	private const float fltDistanceThreshold = .075f;
	private GameObject currentDestination;
	private bool _blnPlayerOnPlatform;
	private int _intDestinationIndex;
	private GameObject[] currentWallSetToMove;
	private bool _blnMovingForward;
	private float fltInitialWaitTime;

	private GameObject _player;


	public enum WallSet{
		startingWallSet,
		endingWallSet
	}
	public WallSet _currentMovingWalls;
	public enum MovingState{
		BeginForward,
		BeginReverse,
		Continue
	}
	public enum States{
		Waiting,
		Delaying,
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
		if (blnTimedPlatform) 
		{
			fltInitialWaitTime = 1;
			_intDestinationIndex = 0;
		}
	}
	
	void Update () {
		switch (currentState) {
		case(States.Waiting):
			if(blnTimedPlatform)
				currentState = States.Delaying;
			break;
		case(States.Delaying):
			if((Time.time >= (fltInitialWaitTime + (intSecondsPerTick*intTicksWaiting))))
				DetermineNextMovement();
			break;
		case(States.RaisingWalls):
			foreach(GameObject wall in currentWallSetToMove)
				if(wall.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Raised)
					if(_currentMovingWalls == WallSet.startingWallSet)
						EnterState_MovingSelf(MovingState.BeginForward);
					else
						EnterState_MovingSelf(MovingState.BeginReverse);
			break;
		case(States.LoweringWalls):
			foreach(GameObject wall in currentWallSetToMove)
				if(wall.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Lowered)
					currentState = States.Waiting;
			break;
		case(States.MovingSelf):

			this.transform.position += fltSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if(_blnPlayerOnPlatform)
				_player.transform.position += fltSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;

			if((this.transform.position - currentDestination.transform.position).magnitude < fltDistanceThreshold)
			{
				fltInitialWaitTime = Time.time;
				currentState = States.Delaying;
			}
			break;
		}
	}

	private void DetermineNextMovement()
	{
		if(blnTimedPlatform)
			if(_intDestinationIndex == checkpoints.Length-1)
				EnterState_MovingSelf(MovingState.BeginReverse);
			else if(_intDestinationIndex == 0)
				EnterState_MovingSelf(MovingState.BeginForward);
			else
				EnterState_MovingSelf (MovingState.Continue);
		else
			if(_intDestinationIndex == checkpoints.Length-1)
				EnterState_LoweringWalls(WallSet.endingWallSet);
			else if(_intDestinationIndex == 0)
				EnterState_LoweringWalls(WallSet.startingWallSet);
			else
				EnterState_MovingSelf(MovingState.Continue);

	}
	
	public void EnterState_RaisingWalls(WallSet wallSetToMove){
		if (wallSetToMove == WallSet.startingWallSet)
			currentWallSetToMove = wallsToMoveStart;
		else
			currentWallSetToMove = wallsToMoveEnd;
		_currentMovingWalls = wallSetToMove;
		foreach (GameObject wall in currentWallSetToMove)
			wall.GetComponent<FSMMoveWall> ().EnterState_Raising ();
		currentState = States.RaisingWalls;
		
	}
	
	public void EnterState_LoweringWalls(WallSet wallSetToMove){
		if (wallSetToMove == WallSet.startingWallSet)
			currentWallSetToMove = wallsToMoveStart;
		else
			currentWallSetToMove = wallsToMoveEnd;
		_currentMovingWalls = wallSetToMove;
		foreach (GameObject wall in currentWallSetToMove)
			wall.GetComponent<FSMMoveWall> ().EnterState_Lowering ();
		currentState = States.LoweringWalls;
		
	}
	
	public void EnterState_MovingSelf(MovingState direction){
		GameObject prevDestination = new GameObject ();

		switch(direction)
		{
			case(MovingState.BeginForward):
				_blnMovingForward = true;
				_intDestinationIndex = 1;
				prevDestination = checkpoints[0];
				break;
			case(MovingState.BeginReverse):
				_blnMovingForward = false;	
				_intDestinationIndex = checkpoints.Length-2;
				prevDestination = checkpoints[checkpoints.Length-1];
				break;
			case(MovingState.Continue):
				prevDestination = checkpoints[_intDestinationIndex];
				if (_blnMovingForward)
					_intDestinationIndex += 1;
				else
					_intDestinationIndex -= 1;
				break;
		}
		currentDestination = checkpoints[_intDestinationIndex];
		if (blnTimedPlatform)
			fltSpeed = (currentDestination.transform.position - prevDestination.transform.position).magnitude / (intTicksMoving * intSecondsPerTick);

		currentState = States.MovingSelf;
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if (other.name == "Sphere") {
			_blnPlayerOnPlatform = true;
			print ("here");
			EnterState_RaisingWalls (WallSet.startingWallSet);
		}
			
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.name == "Sphere") {
						_blnPlayerOnPlatform = false;
						print ("there");
				}
	}
}