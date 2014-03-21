using UnityEngine;
using System.Collections;

public class FSMObstacleThree : MonoBehaviour {
	public GameObject[] movingPlatforms;
	public GameObject[] checkpointLevels;
	
	public enum States{
		Waiting,
		AssigningCheckpoints,
		MovingForward,
		MovingBackward
	}
	public States currentState;
	// Use this for initialization
	void Start () {
		currentState = States.AssigningCheckpoints;
		movingPlatforms[0].GetComponent<FSMMoveGenericPlatform>().checkpoints[0] = checkpointLevels[0].transform.FindChild("Checkpoint_1").gameObject;
		movingPlatforms[1].GetComponent<FSMMoveGenericPlatform>().checkpoints[0] = checkpointLevels[0].transform.FindChild("Checkpoint_2").gameObject;
		movingPlatforms[2].GetComponent<FSMMoveGenericPlatform>().checkpoints[0] = checkpointLevels[0].transform.FindChild("Checkpoint_3").gameObject;
		movingPlatforms[3].GetComponent<FSMMoveGenericPlatform>().checkpoints[0] = checkpointLevels[0].transform.FindChild("Checkpoint_4").gameObject;
		movingPlatforms[4].GetComponent<FSMMoveGenericPlatform>().checkpoints[0] = checkpointLevels[0].transform.FindChild("Checkpoint_5").gameObject;
		AssignCheckpoints (1);
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case(States.AssigningCheckpoints):
			break;


		}
	}

	private void AssignCheckpoints(int intCheckpointLevelGrouping)
	{
		ArrayList randomCheckpoint = new ArrayList();
		randomCheckpoint.Add (1);
		randomCheckpoint.Add (2);
		randomCheckpoint.Add (3);
		randomCheckpoint.Add (4);
		randomCheckpoint.Add (5);
		for(int i = 0; i < movingPlatforms.Length;i++)
		{
			movingPlatforms[i].GetComponent<FSMMoveGenericPlatform>().checkpoints[1] 
			= checkpointLevels[intCheckpointLevelGrouping+i+1].GetComponent<CollectionCheckpointLevelCheckpoints>().checkpoints[i].gameObject;
		}

	}
}
