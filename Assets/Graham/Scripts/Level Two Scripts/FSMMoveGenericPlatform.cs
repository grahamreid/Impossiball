using UnityEngine;
using System.Collections;

public class FSMMoveGenericPlatform : MonoBehaviour {
	
	public GameObject[] checkpoints;
	public GameObject[] wallsToMoveStart;
	public GameObject[] wallsToMoveEnd;
	public GameObject returnCheckpoint;
	
	public  float fltSpeed;
	public bool blnTimedPlatform;
	public float fltTicksMoving;
	public float fltTicksWaiting;
	public float fltSecondsPerTick;
	public float fltDistanceThreshold = .075f;
	
	private const string strPlayerName = "Sphere";
	private GameObject currentDestination;
	private bool _blnPlayerOnPlatform;
	private int _intDestinationIndex;
	private GameObject[] currentWallSetToMove;
	private bool _blnMovingForward;
	private float fltInitialWaitTime;
	
	private GameObject _player;
	private GameObject _sphere;

	private Vector3 prevPosition;
	
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
		RecoveringPlayer
	}
	public States currentState;
	// Use this for initialization
	void Start () {
		States currentState = States.Waiting;	
		_player = GameObject.Find ("Player").gameObject;
		_sphere = _player.transform.FindChild ("Imposi-ball").gameObject.transform.FindChild ("Sphere").gameObject;
		fltInitialWaitTime = 0;
		_intDestinationIndex = 0;
	}
	
	void Update () {
		fltInitialWaitTime += Time.deltaTime;
		switch (currentState) {
		case(States.Waiting):
			if(blnTimedPlatform)
				currentState = States.Delaying;
			break;
		case(States.Delaying):
			if((fltInitialWaitTime >= fltSecondsPerTick*fltTicksWaiting) || !blnTimedPlatform)
					DetermineNextMovement();
			break;
		case(States.RaisingWalls):
			foreach(GameObject wall in currentWallSetToMove)
				if(wall.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Raised)
					if(_currentMovingWalls == WallSet.startingWallSet)
						EnterState_MovingSelf(MovingState.BeginForward);
					else
						EnterState_MovingSelf(MovingState.BeginReverse);
			if(currentWallSetToMove.Length == 0)
				EnterState_MovingSelf (MovingState.BeginForward);
			break;
		case(States.LoweringWalls):
			foreach(GameObject wall in currentWallSetToMove)
				if(wall.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Lowered)
					currentState = States.Waiting;
			if(currentWallSetToMove.Length == 0)
				currentState = States.Waiting;
			break;
		case(States.MovingSelf):
			
			this.transform.position += fltSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if(_blnPlayerOnPlatform)
			{
				_player.transform.position += fltSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
				print("here");
			}
			
			if((this.transform.position - prevPosition).magnitude > (currentDestination.transform.position - prevPosition).magnitude)
			{
				this.transform.position = currentDestination.transform.position;
				fltInitialWaitTime =0;
				currentState = States.Delaying;
			}
			break;
		case(States.RecoveringPlayer):
			this.transform.position += 	((
				(transform.parent.collider.transform.position.y-_sphere.transform.position.y)/(transform.parent.collider.transform.position.y-transform.position.y)
				)*
			                             (
				new Vector3(_sphere.transform.position.x,0,_sphere.transform.position.z)-new Vector3(transform.position.x,0,transform.position.z)
				));
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
		GameObject prevDestination = null;
		switch(direction)
		{
		case(MovingState.BeginForward):
			//if(_intDestinationIndex == checkpoints.Length-1)
				//return;
			_blnMovingForward = true;
			_intDestinationIndex = 1;
			prevDestination = checkpoints[0];
			break;
		case(MovingState.BeginReverse):
			//if(_intDestinationIndex == 0)
				//return;
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
		prevPosition = this.transform.position;
		if (blnTimedPlatform)
			fltSpeed = (currentDestination.transform.position - prevDestination.transform.position).magnitude / (fltTicksMoving * fltSecondsPerTick);
		
		currentState = States.MovingSelf;
	}
	
	public void EnterState_RecoverPlayer()
	{
		if(!_blnPlayerOnPlatform)
			currentState = States.RecoveringPlayer;
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if (other.name == "Sphere") {
			_blnPlayerOnPlatform = true;
			if(!blnTimedPlatform)
				EnterState_RaisingWalls (WallSet.startingWallSet);
		}
		
	}
	
	public void OnTriggerExit(Collider other)
	{
		if (other.name == "Sphere") {
			_blnPlayerOnPlatform = false;
		}
	}
}