﻿using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{

    public int width;   //The number of tiles
    public int height;  //The number of tiles
    private Vector2 posi;
	private Manager.tile[,] map_;
    private int seed_ = 1337; //The room seed
    private int numRandRooms;

	// Use this for initialization
	void Start ()
	{

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void Generate(int width, int height, Vector2 posi)
    {
        this.width = width;
        this.height = height;
        this.map = new Manager.tile[width, height];
        //Start();
        transform.position = posi; //bottom-left origin
    }


    public void Generate(Manager.tile tileType, int width, int height, Vector2 posi )
    {

        if (tileType == Manager.tile.wall)
        {

            //exit if too small
            int minSz = Manager.minRoomSize + 2;
            if (width < minSz || height < minSz)
            {
                string tooSmall = "Room too small, must be at least (" + minSz + "," + minSz + "), but is (" + width + "," + height + ")";
                Debug.LogWarning(tooSmall);
                return;
            }

            //populate data
            this.width = width;
            this.height = height;
			this.map_ = map;
            this.posi = posi; //bottom-left origin
			map_ = new Manager.tile[width,height];

            //store as walls at edges of room
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
						map_[x, y] = Manager.tile.wall;
                    }
                    else
                    {
						map_[x, y] = Manager.tile.nadda;
                    }
                }
            }

        }
        else if (tileType == Manager.tile.floor)
        {

            //exit if too small
            int minSz = Manager.minRoomSize;
            if (width < minSz || height < minSz)
            {
                string tooSmall = "Room too small, must be at least (" + minSz + "," + minSz + "), but is (" + width + "," + height + ")";
                Debug.LogWarning(tooSmall);
                return;
            }

            //store as floor tiles entirely, no walls
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
					map_[x, y] = Manager.tile.floor;
                }
            }

        }
    }


    void CreateRandom()
    {
        
    }

    public void Draw()
    {
        //tiles grow out upwards and rightwards, thus a Room's origin is bottom-left
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
				Manager.tile tile = map_[x, y];
                GameObject what = Manager.tiles[tile];
                Vector3 where = new Vector3(x + posi.x, y + posi.y, 0) * Manager.PixelToUnit;
                Quaternion rot = Quaternion.identity;

                if (what != Manager.tiles[Manager.tile.nadda])
                {
                    Instantiate(what, where, rot);
                }
            }
        }
    }

	public int seed{
		set {seed_ = value;}
		get {return seed_;}
	}

	public Manager.tile[,] map{
		set {map_ = value;}
		get {return map_;}
	}


}
