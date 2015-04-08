using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GenerateStep(ref Manager.tile[,] map, int currentX, int currentY){

		//Debug.Log ("CurrentX: " + currentX + " " + "CurrentY: " + currentY );
		if (map == null)
			return;
		if (map[currentX,currentY] != Manager.tile.nadda )
			return;
		if(!IsOpen(map,currentX,currentY))
			return;
		//if(IsConnector(map,currentX,currentY))
		//	return;
		//Manager.tile[,] directions = 1;
		//Shuffle the directions
		map [currentX, currentY] = Manager.tile.floor;




		Manager.direction[] alpha = new Manager.direction[]{Manager.direction.north, Manager.direction.east, Manager.direction.south, Manager.direction.west};
		//Do a shuffle
		for (int i = 0; i < alpha.Length; i++) {
			Manager.direction temp = alpha[i];
			int randomIndex = Random.Range(i, alpha.Length);
			alpha[i] = alpha[randomIndex];
			alpha[randomIndex] = temp;
		}
		//Then go through the list
		foreach (Manager.direction dir in alpha)
		              {
			switch (dir) {
			case Manager.direction.north: //North
				GenerateStep(ref map, currentX,currentY+1);
				break; 
			case Manager.direction.east: //East
				GenerateStep(ref map, currentX+1,currentY);
				break; 
			case Manager.direction.south: //South
				GenerateStep(ref map, currentX,currentY-1);
				break; 
			case Manager.direction.west: //West
				GenerateStep(ref map, currentX-1,currentY);
				break; 
			default:
				break;
			}
		};
		//Move until you run out of directions
		/*
			switch (Random.Range (1,5)) {
			case Manager.direction.north: //North
				map [currentX, currentY] = Manager.tile.floor;
				GenerateStep(ref map, currentX,currentY+1);
				break; 
			case Manager.direction.east: //East
				map [currentX, currentY] = Manager.tile.floor;
				GenerateStep(ref map, currentX+1,currentY);
				break; 
			case Manager.direction.south: //South
				map [currentX, currentY] = Manager.tile.floor;
				GenerateStep(ref map, currentX,currentY-1);
				break; 
			case Manager.direction.west: //West
				map [currentX, currentY] = Manager.tile.floor;
				GenerateStep(ref map, currentX-1,currentY);
				break; 
			default:
				break;
			}
		*/
		return;

	}
	public void Generate(Room room){
		Manager.tile[,] map = room.map; 
		int seed = room.seed;
		Random.seed = seed;
		int startX=-1,startY=-1;

		//Find an empty spot and don't include the outer walls
		for (int x=1, end=0; x<map.GetLength(0)-1; x++) {
			for (int y=1; y<map.GetLength(1)-1; y++) {
				//Manager.tile.wall
				if (map [x, y] == Manager.tile.nadda) {
					{
						startX  = x;
						startY  = y;
						end = 1;
						break;
					}
				}
			}
			if(end==1) break;
		}
		Debug.Log ("StartX: " + startX + " " + "StartY: " + startY );
		if(startX < 0 || startY <0){//We didn't find any empty spaces done
			return;
		}

		GenerateStep (ref map, startX, startY);

		//Update the map
		room.map = map;
	}
	/*
	public void Generate(Room room){
		Manager.tile[,] map = room.map; 
		int seed = room.seed;
		Manager.direction randomEmptyDirection;
		int startX=-1,startY=-1,currentX=0,currentY=0;
		int random;
		//Random.seed = seed;
		//Find an empty spot and don't include the outer walls
		for (int x=1, end=0; x<map.GetLength(0)-1; x++) {
			for (int y=1; y<map.GetLength(1)-1; y++) {
				//Manager.tile.wall
				if (map [x, y] == Manager.tile.nadda) {
					{
						startX = currentX = x;
						startY = currentY = y;
						end = 1;
						break;
					}
				}
			}
			if(end==1) break;
		}
		Debug.Log ("StartX: " + startX + " " + "StartY: " + startY );
		if(startX < 0 || startY <0){//We didn't find any empty spaces done
				return;
		}
		map [currentX, currentY] = Manager.tile.floor; //Fill the first empty with a floor
		Debug.Log ("CurrentX: " + currentX + " " + "CurrentY: " + currentY );
		//Start filling in the maze
		//Move in a random direction
		for (int i=0; i<1000; i++) {
			Debug.Log ("Step: "+ i + " CurrentX: " + currentX + " " + " CurrentY: " + currentY );
			int modX=currentX, modY=currentY;
			randomEmptyDirection = FindRandomEmptyDirection(map,currentX, currentY);
			if(randomEmptyDirection == Manager.direction.invalid){
				for (int x=1, end=0; x<map.GetLength(0)-1; x++) {
					for (int y=1; y<map.GetLength(1)-1; y++) {
						//Manager.tile.wall
						if (map [x, y] == Manager.tile.nadda) {
							{
								startX = currentX = x;
								startY = currentY = y;
								end = 1;
								break;
							}
						}
					}
					if(end==1) break;
				}
			}
			switch (randomEmptyDirection) {
			case Manager.direction.north: //North
				modX = currentX;
				modY = currentY + 1;
				if(IsInRange(map, modX, modY)){
					//map [modX, modY] = Manager.tile.floor;

					if (IsEmpty (map, modX, modY) && !IsConnector(map, modX,modY)) {
						map [modX, modY] = Manager.tile.floor;

					}
				}
				break; 
			case Manager.direction.east: //East
				modX = currentX + 1;
				modY = currentY;
				if(IsInRange(map, modX, modY)){
					///map [modX, modY] = Manager.tile.floor;
					if (IsEmpty (map, modX, modY) && !IsConnector(map, modX,modY)) {
						map [modX, modY] = Manager.tile.floor;

					}
				}
				break; 
			case Manager.direction.south: //South
				modX = currentX;
				modY = currentY-1;
				if(IsInRange(map, modX, modY)){
					//map [modX, modY] = Manager.tile.floor;
					if (IsEmpty (map, modX, modY) && !IsConnector(map, modX,modY)) {
						map [modX, modY] = Manager.tile.floor;

					}
				}
				break; 
			case Manager.direction.west: //West
				modX = currentX;
				modY = currentY-1;
				if(IsInRange(map, modX, modY)){
					//map [modX, modY] = Manager.tile.floor;
					if (IsEmpty (map, modX, modY) && !IsConnector(map, modX,modY)) {
						map [modX, modY] = Manager.tile.floor;

					}
				}

				break; 
			default:
				break;
			}
			if(IsInRange(map, modX, modY) && !IsConnector(map, modX,modY)){//Is within map and not a connector
				currentX = modX;
				currentY = modY;
			}

		}
		//Update the map
		room.map = map;
	}
*/
	bool IsEmpty(Manager.tile[,] map,int x, int y){ //Is the the tile empty
		if(map[x,y] == Manager.tile.nadda){
			return true;
		}
		return false;
	}

	bool IsOpen(Manager.tile[,] map,int x, int y){
		if (map [x, y] == Manager.tile.nadda) {
			int count=0;
			//Check adjacent
			for (int i=-1; i<2; i++) {
				for (int j=-1; j<2; j++) {
					if(i==x && j==y){
					}else{
						if ((map[x+i,y+j] == Manager.tile.floor || map[x+i,y+j] == Manager.tile.room))//North
							count++;
					}
				}
			}

			if(count>2){
				//Debug.Log (count);
				return false;
			}
			return true;
		}
		return false;
	}

	Manager.direction FindRandomEmptyDirection(Manager.tile[,] map,int x, int y){
		bool north	=	IsEmpty(map,x,y+1),
			east	=	IsEmpty(map,x+1,y), 
			south	= 	IsEmpty(map,x,y-1), 
			west	=	IsEmpty(map,x-1,y),
			end = false;
		int random;

		if( !north && !east && !south && !west) // we reached a dead end
			return Manager.direction.invalid;

		while (end == false) {
			random = Random.Range(0,4);
			switch (random) {
				case (int)Manager.direction.north: //North
					if(north == true)
						return Manager.direction.north;
					end = true;
				break; 
				case (int)Manager.direction.east: //East
					if(east == true)
						return Manager.direction.east;
					end = true;
	
				break; 
				case (int)Manager.direction.south: //South
					if(south == true)
						return Manager.direction.south;
					end = true;
				break; 
				case (int)Manager.direction.west: //West
					if(west == true)
						return Manager.direction.west;
					end = true;
				break; 
				default:
				break;
			}

		}
		return Manager.direction.invalid;

	}

	bool IsConnector(Manager.tile[,] map,int x, int y){ //Does the tile connect to any other tile, true if it would contain two floors if removed
		int count=0;
		//return false;//disabled for now
		/*
		for (int i=-1; i<2; i++) {
			for (int j=-1; j<2; j++) {
				if(i==x && j==y){
				}else{
				if (map[x+i,y+j] == Manager.tile.wall || map[x+i,y+j] == Manager.tile.nadda)//North
					count++;
				}
			}
		}*/
		Manager.tile wall = Manager.tile.wall;
		Manager.tile floor = Manager.tile.wall;
		if ((map[x-1,y] == wall || map[x-1,y] == floor) && (map[x+1,y] == wall || map[x+1,y] == floor) ) {//West and East
			count++;
		}
		if ((map[x-1,y] == wall || map[x-1,y] == floor) && (map[x,y+1] == wall || map[x,y+1] == floor)) {//West and North
			count++;
		}
		if ((map[x-1,y] == wall || map[x-1,y] == floor) && (map[x,y-1] == wall || map[x,y-1] == floor )) {//West and South
			count++;
		}
		if ((map[x,y+1] == wall || map[x,y+1] == floor)&& (map[x+1,y] == wall || map[x+1,y] == floor)) {//North and East
			count++;
		}
		if ((map[x,y+1] == wall || map[x,y+1] == floor) && (map[x,y-1] == wall || map[x,y-1] == floor)) {//North and South
			count++;
		}
		if ((map[x+1,y] == wall || map[x+1,y] == floor)&& (map[x,y-1] == wall || map[x,y-1] == floor)) {//East and South
			count++;
		}




		if(count > 1){
			Debug.Log("IsConnector");
			return true;
		}
		Debug.Log("Connections: " + count);
		return false;
	}

	bool IsDeadEnd(Manager.tile[,] map,int x, int y){ //Is the tile a dead end, true if 3 of 4 adjacent are walls
		int count=0;
		if (map[x,y+1] == Manager.tile.wall)//North
			count++;
		if (map[x+1,y] == Manager.tile.wall)//East
			count++;
		if (map[x,y-1] == Manager.tile.wall)//South
			count++;
		if (map[x-1,y] == Manager.tile.wall)//West
			count++;
		if(count == 3){
			return true;
		}
		return false;
	}

	bool IsInRange(Manager.tile[,] map,int x, int y){
		if (x < 0 || x > map.GetLength (0)-1)
			return false;
		if (y < 0 || y > map.GetLength (1)-1)
			return false;
		return true;
	}
}
