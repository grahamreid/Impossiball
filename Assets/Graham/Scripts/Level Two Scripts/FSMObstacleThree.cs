using UnityEngine;
using System.Collections;

public class FSMObstacleThree : MonoBehaviour {
	public GameObject[] movingPlatforms;
	public GameObject[] checkpointLevels;
	private int intCheckpointCount;
	private int intCheckpointLevelsCount;
	
	public enum States{
		Waiting,
		AssigningCheckpoints,
		MovingForward,
		MovingBackward
	}
	public States currentState;
	// Use this for initialization
	void Start () {
		intCheckpointCount = movingPlatforms [0].GetComponent<FSMMoveGenericPlatform> ().checkpoints.Length;
		intCheckpointLevelsCount = checkpointLevels.Length;
		print (intCheckpointCount);
		currentState = States.AssigningCheckpoints;
		movingPlatforms[0].GetComponent<FSMMoveGenericPlatform>().checkpoints[1] = checkpointLevels[0].transform.FindChild("Checkpoint_1").gameObject;
		movingPlatforms[1].GetComponent<FSMMoveGenericPlatform>().checkpoints[1] = checkpointLevels[0].transform.FindChild("Checkpoint_2").gameObject;
		movingPlatforms[2].GetComponent<FSMMoveGenericPlatform>().checkpoints[1] = checkpointLevels[0].transform.FindChild("Checkpoint_3").gameObject;
		movingPlatforms[3].GetComponent<FSMMoveGenericPlatform>().checkpoints[1] = checkpointLevels[0].transform.FindChild("Checkpoint_4").gameObject;
		movingPlatforms[4].GetComponent<FSMMoveGenericPlatform>().checkpoints[1] = checkpointLevels[0].transform.FindChild("Checkpoint_5").gameObject;
		AssignCheckpoints (2,1);
		AssignCheckpoints (4,3);
		AssignCheckpoints (6,5);
		AssignCheckpoints (8,7);
		AssignCheckpoints (10,9);
		AssignCheckpoints (12,11);
		AssignCheckpoints (14,13);
		movingPlatforms[0].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-1] = checkpointLevels[intCheckpointLevelsCount-1].transform.FindChild(movingPlatforms[0].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-3].transform.gameObject.name).gameObject;
		movingPlatforms[1].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-1] = checkpointLevels[intCheckpointLevelsCount-1].transform.FindChild(movingPlatforms[1].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-3].transform.gameObject.name).gameObject;
		movingPlatforms[2].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-1] = checkpointLevels[intCheckpointLevelsCount-1].transform.FindChild(movingPlatforms[2].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-3].transform.gameObject.name).gameObject;
		movingPlatforms[3].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-1] = checkpointLevels[intCheckpointLevelsCount-1].transform.FindChild(movingPlatforms[3].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-3].transform.gameObject.name).gameObject;
		movingPlatforms[4].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-1] = checkpointLevels[intCheckpointLevelsCount-1].transform.FindChild(movingPlatforms[4].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointCount-3].transform.gameObject.name).gameObject;
		currentState = States.MovingForward;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case(States.AssigningCheckpoints):
			break;
		case(States.MovingForward):
			foreach(GameObject platform in movingPlatforms)
				platform.GetComponent<FSMMoveGenericPlatform>().blnTimedPlatform = true;
			currentState = States.Waiting;
			break;


		}
	}

	private void AssignCheckpoints(int intCheckpointIndex, int intCheckpointLevelGrouping)
	{
		ArrayList randomCheckpoint = new ArrayList();
		randomCheckpoint.Add (0);
		randomCheckpoint.Add (1);
		randomCheckpoint.Add (2);
		randomCheckpoint.Add (3);
		randomCheckpoint.Add (4);

		ArrayList randomCheckpointLevel = new ArrayList();
		randomCheckpointLevel.Add (0);
		randomCheckpointLevel.Add (1);
		randomCheckpointLevel.Add (2);
		randomCheckpointLevel.Add (3);
		randomCheckpointLevel.Add (4);
		for(int i = 0; i < movingPlatforms.Length;i++)
		{
			int intRandom = Random.Range(0,randomCheckpoint.Count);
			int intRandomLevel = Random.Range(0,randomCheckpointLevel.Count);
			string strLastCheckpointName = movingPlatforms[i].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointIndex-1].transform.gameObject.name;
			//print ("level: " + (intCheckpointLevelGrouping+(int)randomCheckpointLevel[intRandomLevel]));

			movingPlatforms[i].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointIndex] 
			= checkpointLevels[intCheckpointLevelGrouping+(int)randomCheckpointLevel[intRandomLevel]].transform.FindChild(strLastCheckpointName).gameObject;

			movingPlatforms[i].GetComponent<FSMMoveGenericPlatform>().checkpoints[intCheckpointIndex+1] 
			= checkpointLevels[intCheckpointLevelGrouping+(int)randomCheckpointLevel[intRandomLevel]].GetComponent<CollectionCheckpointLevelCheckpoints>().checkpoints[(int)randomCheckpoint[intRandom]].gameObject;
			randomCheckpoint.RemoveAt(intRandom);
			randomCheckpointLevel.RemoveAt(intRandomLevel);
		}

	}
}
