﻿using UnityEngine;
using System.Collections;

public class FSMMovePlatform : MonoBehaviour {
	
	public  int intSpeed;
	public GameObject objDestination;
	private GameObject objWallToMove;
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
	enum State{
		Waiting,
		RaisingWalls,
		LoweringWalls,
		MovingSelf,
		Lights,
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
		PlatformWall1 = this.transform.FindChild ("PlatformWall1").gameObject;
		PlatformWall2 = this.transform.FindChild ("PlatformWall2").gameObject;
		PlatformWall3 = this.transform.FindChild ("PlatformWall3").gameObject;
		PlatformWall4 = this.transform.FindChild ("PlatformWall4").gameObject;
		PlatformWall4 = this.transform.FindChild ("PlatformWall4").gameObject;
		Elevator = this.transform.FindChild ("Elevator").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	switch (currentState) {
		case(State.Waiting):
				break;		
		case(State.RaisingWalls):
			if(PlatformWall1.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Waiting)
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
			Elevator.GetComponent<FSMMoveElevator>().EnterState_Illuminating(Time.time);
			currentState = State.PendingElevator;
			break;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.name == strPlayerName)
			switch(currentState){
				case(State.Waiting):
					PlatformWall1.GetComponent<FSMMoveWall>().EnterState_Raising();
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
