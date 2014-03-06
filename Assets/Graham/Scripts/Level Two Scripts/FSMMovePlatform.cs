using UnityEngine;
using System.Collections;

public class FSMMovePlatform : MonoBehaviour {
	
	public  int intSpeed;
	public GameObject objDestination;
	public GameObject objWallToRaise;
	public GameObject objWallToDrop;
	private float fltReferenceHeight;
	private string strDestinationName;
	private const string strPlayerName = "Sphere";
	private const float fltDistanceThreshold = .1f;

	private GameObject Elevator;

	private GameObject 	LightGroup1,
						LightGroup2,
						LightGroup3,
						LightGroup4;
	private GameObject  PlatformWall1,
						PlatformWall2,
						PlatformWall3,
						PlatformWall4;
	private GameObject _player;
	enum State{
		Waiting,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
		LightsOn,
		LightsOff,
		PendingElevator
	}
	State currentState;
	// Use this for initialization
	void Start () {
		currentState = State.Waiting;	
		strDestinationName = objDestination.name;
		LightGroup1 = this.transform.FindChild ("CornerLightGroup1").gameObject;
		LightGroup2 = this.transform.FindChild ("CornerLightGroup2").gameObject;
		LightGroup3 = this.transform.FindChild ("CornerLightGroup3").gameObject;
		LightGroup4 = this.transform.FindChild ("CornerLightGroup4").gameObject;
		Elevator = this.transform.FindChild ("Elevator").gameObject;
		_player = GameObject.Find ("Player").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	switch (currentState) {
		case(State.Waiting):
			if( this.transform.FindChild("Elevator").GetComponent<FSMMoveElevator>().currentState == FSMMoveElevator.States.MovingSelf)
			   currentState = State.LightsOff;
			break;		
		case(State.RaisingWalls):
			if(objWallToRaise.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Raised)
				currentState = State.MovingSelf;
			break;
		case(State.MovingSelf):
			this.transform.position += intSpeed * (objDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			_player.transform.position += intSpeed * (objDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if((this.transform.position - objDestination.transform.position).magnitude < fltDistanceThreshold)
			{
				objWallToDrop.GetComponent<FSMMoveWall>().EnterState_Lowering();
				currentState = State.LightsOn;
			}
			break;
		case(State.LightsOn):
			LightGroup1.GetComponent<FSMLights>().EnterState_ShineLights();
			LightGroup2.GetComponent<FSMLights>().EnterState_ShineLights();
			LightGroup3.GetComponent<FSMLights>().EnterState_ShineLights();
			LightGroup4.GetComponent<FSMLights>().EnterState_ShineLights();
			Elevator.GetComponent<FSMMoveElevator>().EnterState_Illuminating(Time.time);
			currentState = State.Waiting;
			break;
	   case(State.LightsOff):
		   LightGroup1.GetComponent<FSMLights>().EnterState_DisableLights();
		   LightGroup2.GetComponent<FSMLights>().EnterState_DisableLights();
		   LightGroup3.GetComponent<FSMLights>().EnterState_DisableLights();
		   LightGroup4.GetComponent<FSMLights>().EnterState_DisableLights();
		   currentState = State.Waiting;
		   break;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.name == strPlayerName)
			switch(currentState){
				case(State.Waiting):
					objWallToRaise.GetComponent<FSMMoveWall>().EnterState_Raising();
					currentState = State.RaisingWalls;
					break;
			}
		if(other.name == strDestinationName)
			switch(currentState){
				case(State.MovingSelf):
					currentState = State.Waiting;
					break;	
			}
	}

}
