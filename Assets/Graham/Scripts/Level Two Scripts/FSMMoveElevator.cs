﻿using UnityEngine;
using System.Collections;

public class FSMMoveElevator : MonoBehaviour {
	public  int intSpeed;
	public GameObject objDestination;
	public GameObject objCheckpoint;
	public GameObject wallToDrop;
	private GameObject objWallToMove;
	private float _fltReferenceHeight;
	private string _strDestinationName;
	private const string strPlayerName = "Sphere";
	private Renderer[] ElevatorChildrenWallRenderers;
	private float _fltLightsTimer;
	private float _fltDelayBeforeHighlightingWalls = 1.4f;
	private const float fltDistanceThreshold = .1f;

	private GameObject _player;

	enum States{
		Waiting,
		Illuminating,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
		ReachedTop
	}
	States currentState;
	// Use this for initialization
	void Start () {
		States currentState = States.Waiting;	
		_strDestinationName = objDestination.name;
		ElevatorChildrenWallRenderers = this.GetComponentsInChildren<Renderer>();
		this.collider.enabled = false;
		_player = GameObject.Find ("Player").gameObject;
	}
	
	void Update () {
		switch (currentState) {
		case(States.Waiting):
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
				if(child.gameObject.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Waiting)
					currentState = States.MovingSelf;
			break;
		case(States.MovingSelf):
			this.transform.position += intSpeed * (objDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			_player.transform.position += intSpeed * (objDestination.transform.position - this.transform.position).normalized * Time.deltaTime;
			if((this.transform.position - objDestination.transform.position).magnitude < fltDistanceThreshold)
				EnterState_ReachedTop();
			break;
		case(States.ReachedTop):
			if(wallToDrop.gameObject.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Waiting)
				currentState = States.Waiting;
			break;
		}
	}
	
	public void EnterState_RaisingWalls(){
		currentState = States.RaisingWalls;

	}

	public void EnterState_LoweringWalls(){
		currentState = States.LoweringWalls;
		
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
				this.collider.enabled = false;
				currentState = States.RaisingWalls;
				foreach (Transform child in this.transform.FindChild("ElevatorFloor").transform)
					child.gameObject.GetComponent<FSMMoveWall>().EnterState_Raising();
				break;
				
		}
	}
}
