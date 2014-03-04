using UnityEngine;
using System.Collections;

public class FSMMoveElevator : MonoBehaviour {
	public  int intSpeed;
	public GameObject objCheckpointDestination;
	public GameObject objCheckpoint;
	public GameObject wallToDrop;
	private GameObject objWallToMove;
	private float _fltReferenceHeight;
	private string _strDestinationName;
	private const string strPlayerName = "Sphere";
	private Renderer[] ElevatorChildrenWallRenderers;
	private float _fltLightsTimer;
	private float _fltDelayBeforeHighlightingWalls = 1.4f;
	private const float fltDistanceThreshold = .05f;
	private GameObject currentDestination;
	private bool _blnPlayerOnPlatform;

	private GameObject _player;
	private GameObject _elevatorReturnPoint;

	public enum States{
		Waiting,
		Illuminating,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
		ReachedTop
	}
	public States currentState;
	// Use this for initialization
	void Start () {
		States currentState = States.Waiting;	
		_strDestinationName = objCheckpointDestination.name;
		ElevatorChildrenWallRenderers = this.GetComponentsInChildren<Renderer>();
		this.collider.enabled = false;
		_player = GameObject.Find ("Player").gameObject;
		_elevatorReturnPoint = this.transform.parent.transform.FindChild ("ElevatorReturnPoint").gameObject;
	}
	
	void Update () {
		switch (currentState) {
		case(States.Waiting):
			if((this.transform.position - objCheckpointDestination.transform.position).magnitude < fltDistanceThreshold)
				EnterState_ReachedTop();	
			break;	
		case(States.Illuminating):
			if((Time.time - _fltLightsTimer) >= _fltDelayBeforeHighlightingWalls)
			{
				foreach(Renderer rend in ElevatorChildrenWallRenderers)
					rend.material.color = Color.green;
				this.transform.FindChild("ElevatorFloor").renderer.material.color = Color.grey;	
				this.collider.enabled = true;
				currentState = States.Waiting;
			}
			break;
		case(States.RaisingWalls):
			foreach(Transform child in this.transform.FindChild("ElevatorFloor").transform)
				if(child.gameObject.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Raised)
					EnterState_MovingSelf(objCheckpointDestination,true);
			break;
		case(States.LoweringWalls):
			foreach(Transform child in this.transform.FindChild("ElevatorFloor").transform)
				if(child.gameObject.GetComponent<FSMMoveWall>().currentState != FSMMoveWall.State.Lowered)
					return;
			EnterState_MovingSelf(_elevatorReturnPoint,false);
			break;
		case(States.MovingSelf):
			this.transform.position += intSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if(_blnPlayerOnPlatform)
				_player.transform.position += intSpeed * (currentDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if((this.transform.position - currentDestination.transform.position).magnitude < fltDistanceThreshold)
				currentState = States.Waiting;
			break;
		case(States.ReachedTop):
			if(wallToDrop.gameObject.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Lowered)
				currentState = States.Waiting;
			break;
		}
	}
	
	public void EnterState_RaisingWalls(){
		this.collider.enabled = false;
		foreach (Transform child in this.transform.FindChild("ElevatorFloor").transform)
			child.gameObject.GetComponent<FSMMoveWall>().EnterState_Raising();
		currentState = States.RaisingWalls;

	}

	public void EnterState_LoweringWalls(){
		this.collider.enabled = true;
		foreach (Transform child in this.transform.FindChild("ElevatorFloor").transform) {
			child.gameObject.GetComponent<FSMMoveWall> ().EnterState_Lowering ();
			child.gameObject.renderer.material.color = Color.grey;
		}
		currentState = States.LoweringWalls;
		
	}

	public void EnterState_MovingSelf(GameObject destination,bool blnPlayerOnPlatform){
		currentDestination = destination;
		_blnPlayerOnPlatform = blnPlayerOnPlatform;
		currentState = States.MovingSelf;
	}

	public void EnterState_ReachedTop(){
		objCheckpoint.GetComponent<FSMCheckpointPlatform>().EnterState_LoweringWalls();
		wallToDrop.GetComponent<FSMMoveWall>().EnterState_Lowering();
		currentState = States.ReachedTop;
	}

	public void EnterState_Illuminating(float fltLightsTimer){
		_fltLightsTimer = fltLightsTimer;
		currentState = States.Illuminating;
	}

	public void OnTriggerEnter(Collider other)
	{
		switch (currentState) {
			case(States.Waiting):
				EnterState_RaisingWalls();
				break;
				
		}
	}
}
