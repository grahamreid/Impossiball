using UnityEngine;
using System.Collections;

public class FSMMovePlatform : MonoBehaviour {
	
	public  int intSpeed;
	public GameObject objDestination;
	private GameObject objWallToMove;
	private float fltReferenceHeight;
	private string strDestinationName;
	private const string strPlayerName = "Sphere";
	private const float fltDistanceThreshold = .1f;
	private Renderer[] ElevatorChildrenWallRenderers;

	float fltOngoingTimer = 0;
	float fltDelayBeforeHighlightingWalls = 1.4f;

	private GameObject 	LightGroup1,
						LightGroup2,
						LightGroup3,
						LightGroup4;
	enum State{
		Waiting,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
		Lights,
		WaitingOnLights,
		Finished
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
		ElevatorChildrenWallRenderers = this.transform.FindChild ("Elevator").GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
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
			if((this.transform.position - objDestination.transform.position).magnitude < fltDistanceThreshold)
				currentState = State.Lights;
			break;
		case(State.Lights):
			LightGroup1.GetComponent<FSMLights>().EnterState_ShineLights();
			LightGroup2.GetComponent<FSMLights>().EnterState_ShineLights();
			LightGroup3.GetComponent<FSMLights>().EnterState_ShineLights();
			LightGroup4.GetComponent<FSMLights>().EnterState_ShineLights();
			fltOngoingTimer = Time.time;
			currentState = State.WaitingOnLights;
			break;
		case(State.WaitingOnLights):
			if((Time.time - fltOngoingTimer) >= fltDelayBeforeHighlightingWalls)
			{
				foreach(Renderer rend in ElevatorChildrenWallRenderers)
					rend.material.color = Color.green;
				this.transform.FindChild("Elevator").renderer.material.color = Color.grey;
				currentState = State.Finished;
			}
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

	void ShineLights()
	{

	}
}
