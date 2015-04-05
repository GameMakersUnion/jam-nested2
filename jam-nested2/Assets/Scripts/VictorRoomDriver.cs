using UnityEngine;
using System.Collections;

public class VictorRoomDriver : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		//Room room = new Room();
		int width = 20;
		int height = 20; 
		Vector2 posi = new Vector2(0,0);
		GameObject go = (GameObject)Instantiate( Resources.Load("tiles/room"), new Vector3(0, 0, 0), Quaternion.identity);
		Room room = go.GetComponent<Room>();
		room.Generate(Manager.tile.wall, width, height, posi );
		MazeGenerator maze = new MazeGenerator ();
		maze.Generate(room);
		room.Draw();
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
