using UnityEngine;
using System.Collections;

public class FSMLights : MonoBehaviour {
	
	float fltOngoingTimer = 0;
	const float fltLightTimingThreshold = 0.2f;
	enum State{
		Waiting,
		ShineLights
	}
	GameObject 	Light1,
				Light2,
				Light3,
				Light4,
				Light5,
				Light6,
				Light7;
	State currentState;
	// Use this for initialization
	void Start () {
		currentState = State.Waiting;
		Light1 = this.transform.FindChild ("Light1").gameObject;
		Light2 = this.transform.FindChild ("Light2").gameObject;
		Light3 = this.transform.FindChild ("Light3").gameObject;
		Light4 = this.transform.FindChild ("Light4").gameObject;
		Light5 = this.transform.FindChild ("Light5").gameObject;
		Light6 = this.transform.FindChild ("Light6").gameObject;
		Light7 = this.transform.FindChild ("Light7").gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		switch(currentState){
			case(State.Waiting):
				break;
			case(State.ShineLights):
				float fltTimeDiff = Time.time - fltOngoingTimer;
				if(fltTimeDiff < fltLightTimingThreshold)
					switchOnLight(Light1);
				else if(fltTimeDiff < fltLightTimingThreshold*2)
					switchOnLight(Light2);
				else if(fltTimeDiff < fltLightTimingThreshold*3)
					switchOnLight(Light3);
				else if(fltTimeDiff < fltLightTimingThreshold*4)
					switchOnLight(Light4);
				else if(fltTimeDiff < fltLightTimingThreshold*5)
					switchOnLight(Light5);
				else if(fltTimeDiff < fltLightTimingThreshold*6)
					switchOnLight(Light6);
				else if(fltTimeDiff < fltLightTimingThreshold*7)
					switchOnLight(Light7);
				break;
		}
	}

	public void EnterState_ShineLights()
	{
		fltOngoingTimer = Time.time;
		currentState = State.ShineLights;
	}

	void switchOnLight(GameObject objLight){
		(objLight.GetComponent ("Halo") as Behaviour).enabled = true;
		objLight.renderer.material.color = Color.green;
	}

}
