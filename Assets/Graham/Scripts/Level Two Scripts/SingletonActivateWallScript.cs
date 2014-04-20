using UnityEngine;
using System.Collections;

public class SingletonActivateWallScript : MonoBehaviour {

	public GameObject[] Walls;
	// Use this for initialization
	void Start () {
		foreach(GameObject objWall in Walls)
		{
			objWall.GetComponent<FSMMoveGenericPlatform>().enabled = false;
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		foreach(GameObject objWall in Walls)
		{
			objWall.GetComponent<FSMMoveGenericPlatform>().enabled = true;
		}
	}
}
