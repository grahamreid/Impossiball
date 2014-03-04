using UnityEngine;
using System.Collections;

public class FSMMoveWall : MonoBehaviour {

	float fltReferenceHeight;

	public enum State{
		Lowered,
		Raised,
		Raising,
		Lowering
	}
	public State currentState;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case(State.Raising):
			this.transform.position += new Vector3(0,this.transform.localScale.y,0)*Time.deltaTime;
			if(this.transform.position.y >= fltReferenceHeight+this.transform.localScale.y)
				currentState = State.Raised;
			break;
		case(State.Lowering):
			this.transform.position -= new Vector3(0,this.transform.localScale.y,0)*Time.deltaTime;
			if(this.transform.position.y <= fltReferenceHeight-this.transform.localScale.y)
				currentState = State.Lowered;
			break;
		}
	}

	public void EnterState_Raising()
	{
		if (currentState != State.Raised) {
			this.fltReferenceHeight = this.transform.position.y;
			currentState = State.Raising;
		}
	}

	public void EnterState_Lowering()
	{
		if (currentState != State.Lowered){
			this.fltReferenceHeight = this.transform.position.y;
			currentState = State.Lowering;
		}
	}
}
