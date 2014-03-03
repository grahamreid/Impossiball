using UnityEngine;
using System.Collections;

public class FSMMoveWall : MonoBehaviour {

	float fltReferenceHeight;

	public enum State{
		Waiting,
		Raising,
		Lowering
	}
	public State currentState;
	// Use this for initialization
	void Start () {
		currentState = State.Waiting;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case(State.Raising):
			this.transform.position += new Vector3(0,this.transform.localScale.y,0)*Time.deltaTime;
			if(this.transform.position.y >= fltReferenceHeight+this.transform.localScale.y)
				currentState = State.Waiting;
			break;
		case(State.Lowering):
			if(this.transform.position.y <= fltReferenceHeight-this.transform.localScale.y)
				currentState = State.Waiting;
			break;
		}
	}

	public void EnterState_Raising()
	{
		this.fltReferenceHeight = this.transform.position.y;
		currentState = State.Raising;
	}

	public void EnterState_Lowering()
	{
		this.fltReferenceHeight = this.transform.position.y;
		currentState = State.Lowering;
	}
}
