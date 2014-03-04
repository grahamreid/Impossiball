using UnityEngine;
using System.Collections;

public class FSMCheckpointPlatform : MonoBehaviour {
	public GameObject wallToDrop;
	public GameObject elevator;
	
	private GameObject _player;
	
	enum States{
		Waiting,
		RaisingWalls,
		LoweringWalls,
	}
	States currentState;
	// Use this for initialization
	void Start () {
		States currentState = States.Waiting;	
		_player = GameObject.Find ("Player").gameObject;
	}
	
	void Update () {
				switch (currentState) {
					case(States.Waiting):
						break;	
					case(States.RaisingWalls):
						if(wallToDrop.GetComponent<FSMMoveWall>().currentState == FSMMoveWall.State.Raised)
							currentState = States.Waiting;
						break;
				}
	}

	public void EnterState_RaisingWalls(){
		wallToDrop.GetComponent<FSMMoveWall>().EnterState_Raising();
		currentState = States.RaisingWalls;
		
	}
	
	public void EnterState_LoweringWalls(){
		wallToDrop.GetComponent<FSMMoveWall>().EnterState_Lowering();
		currentState = States.LoweringWalls;
		
	}

	public void OnTriggerEnter(Collider other)
	{
		switch(other.name){
			case("Sphere"):
				this.collider.enabled = false;
				elevator.GetComponent<FSMMoveElevator>().EnterState_LoweringWalls();
				break;
		}
	}
}
