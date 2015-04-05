using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{

    private int width_;   //The number of tiles
    private int height_;  //The number of tiles
    private Vector2 posi_;
	private Manager.tile[,] map_;
    private int seed_ = 1337; //The room seed
    private int numRandRooms_;
    private static int childCounter_ = 0;


    public int seed{
		set {seed_ = value;}
		get {return seed_;}
	}

	public Manager.tile[,] map{
		set {map_ = value;}
		get {return map_;}
	}

    public int width {
        set { if (value >= 2) width_ = value; else Debug.LogWarning("Invalid size, too small.");}
        get { return width_; }
    }

    public int height
    {
        set { if (value >= 2) height_ = value; else Debug.LogWarning("Invalid size, too small."); }
        get { return height_; }
    }

	// Use this for initialization
	void Start ()
	{

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //the top-level main outer room, publically callable
    public void Generate( int width, int height, Vector2 posiWorld )
    {
        Random.seed = seed_;

        //exit if too small
        int minSz = Manager.minRoomSize + 2;
        if (width < minSz || height < minSz)
        {
            string tooSmall = "Room too small, must be at least (" + minSz + "," + minSz + "), but is (" + width + "," + height + ")";
            Debug.LogWarning(tooSmall);
            return;
        }

        //populate data
        this.width_ = width;
        this.height_ = height;
		//this.map_ = map;
        this.posi_ = posiWorld; //bottom-left origin
		this.map_ = new Manager.tile[width,height];
        numRandRooms_ = Random.Range(Manager.minRandRooms, Manager.maxRandRooms);

        //create empty walls object
        GameObject walls = new GameObject("walls");
        walls.transform.parent = transform;

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

        //create empty rooms object
        GameObject rooms = new GameObject("rooms");
        rooms.transform.parent = this.transform;

        CreateChildren();
        //Redrwa
        Draw();

    }

    //one of the contained rooms inside a main room, internal only
    private void GenerateChild(int width, int height){
            
        ////exit if too small
        //int minSz = Manager.minRoomSize;
        //if (width_ < minSz || height_ < minSz)
        //{
        //    string tooSmall = "Room too small, must be at least (" + minSz + "," + minSz + "), but is (" + width_ + "," + height_ + ")";
        //    Debug.LogWarning(tooSmall);
        //    return;
        //}


        int numAttempts = 20;

        while (numAttempts > 0)
        {
            //posiWorld
            int minX = 1; //inside wall
            int maxX = this.width_ - width;
            int minY = 1; //inside wall
            int maxY = this.height_ - height;

            //origin of child room relative to parent
            int tryPosiX = Random.Range(minX, maxX);
            int tryPosiY = Random.Range(minY, maxY);

            Debug.Log("trying child at (" + (tryPosiX + width) + "," +  (tryPosiY + height) + "), size (" + width +","+ height+")");

            bool collisionsFound = CheckCollisions(tryPosiX, tryPosiY, width, height);

            //does it fit?
            //if (tryPosiX + width <= this.width_ && tryPosiY + height <= this.height_)
            if (collisionsFound == false)
            {

                //store as floor tiles entirely, overwrite any walls or anything existing
                Manager.tile[,] childMap = new Manager.tile[width,height];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        map_[tryPosiX + x, tryPosiY + y] = Manager.tile.room;
                        childMap[x, y] = Manager.tile.floor;
                    }
                }

                //make child room
                GameObject go = (GameObject)Instantiate(Resources.Load("tiles/room"), new Vector3(tryPosiX, tryPosiY, 0) * Manager.PixelToUnit, Quaternion.identity);
                go.name = "Room" + childCounter_++;
                go.transform.parent = transform.FindChild("rooms");
                Room room = go.GetComponent<Room>();
                room.Populate(width, height, this.posi_ + new Vector2(tryPosiX, tryPosiY), childMap);
                room.Draw();

                break;
            }
            //try again!
            else
            {
                numAttempts--;
            }

        }

        //Vector2 minRoomPosi = new Vector2 (0,0); //of the parent
        //Vector2 maxRoomSize = new Vector2( this.width_ - width_, this.height_ - height_ ); //within the parent

        //Vector2 tryPosi = new Vector2(minRoomPosi.x, minRoomPosi.y);
        //Vector2 trySize = new Vector2(maxRoomSize.x, maxRoomSize.y);


    }

    //only called from parent
    private void CreateChildren()
    {
        //recursive
        if ( numRandRooms_ > 0)
        {
            const int minRandRoomSz = 2;
            int maxRandRoomSz = (int)((this.height_ + this.width_)/2 * .3f); //30% 
            int tryWidth = Random.Range(minRandRoomSz, maxRandRoomSz);
            int tryHeight = Random.Range(minRandRoomSz, maxRandRoomSz);

            //Debug.Log("trying child sized " + tryWidth+","+ tryHeight);

            GenerateChild(tryWidth, tryHeight);
            numRandRooms_--;

            CreateChildren();

        }
    }

    private bool CheckCollisions(int tryPosiX, int tryPosiY, int width, int height)
    {
        string dbgStr = "";
        //does it not collide with any rooms?
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Manager.tile tryTile = map_[tryPosiX + x, tryPosiY + y];
                dbgStr += tryTile + "(" + (tryPosiX + x) + ":" + (tryPosiY + y) + "),";
                if (tryTile == Manager.tile.room)
                {
                    Debug.Log(dbgStr + "COLLISION!");
                    return true;
                }

            }
            dbgStr += "\n";
        }
        Debug.Log(dbgStr);
        return false;
    }

    public void Populate(int width, int height, Vector2 posi, Manager.tile[,] map)
    {
        this.width_ = width;
        this.height_ = height;
        this.posi_ = posi;
        this.map = map;
    }

    //also does naming and parenting, may as well since intantiation occurs here.
    public void Draw()
    {
        Transform walls = transform.FindChild("walls");
        Manager.hue hue = (Manager.hue) Manager.GetNextColor();
        int count = 0;

        //tiles grow out upwards and rightwards, thus a Room's origin is bottom-left
        for (int x = 0; x < width_; x++)
        {
            for (int y = 0; y < height_; y++)
            {
				Manager.tile tile = map_[x, y];
                GameObject what = Manager.tiles[tile];
                Vector3 where = new Vector3(x + posi_.x, y + posi_.y, 0) * Manager.PixelToUnit;
                Quaternion rot = Quaternion.identity;

                if (what != Manager.tiles[Manager.tile.nadda] && what != Manager.tiles[Manager.tile.room])
                {
                    GameObject go = (GameObject)Instantiate(what, where, rot);
                    if (what == Manager.tiles[Manager.tile.wall])
                    {
                        go.transform.parent = walls;
                    }
                    //since this is a room object, and it's nestable, this makes sense.
                    else if (what == Manager.tiles[Manager.tile.floor])
                    {
                        go.transform.parent = this.transform;
                    }
                    go.name = what.ToString().Split(' ')[0] + count++;
                    go.GetComponent<SpriteRenderer>().color = Manager.hues[hue];
                }
            }
        }
    }







}
