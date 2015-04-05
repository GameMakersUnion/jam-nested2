using System;
using UnityEngine;
using System.Collections.Generic;

public static class Manager
{

    public const int PixelToUnit = 32; //scale to multiple all instantiations in space upon
    
    public enum hue { Red, Orange, Yellow, GreenLight, Green, BlueLight, Blue, Violet, Lavender }

    public static Dictionary<hue, Color32> hues = new Dictionary<hue, Color32>()
    {
        {hue.Red, new Color(1, 0, 0, 1)},
        {hue.Orange, new Color(1, 116f/225f, 0, 1)},
        {hue.Yellow, new Color(1,1, 136/255, 1)},   //looks better with ints rounded
        {hue.GreenLight, new Color(140f/255f, 1, 0, 1)},
        {hue.Green, new Color(0,140f/255f, 0, 1)},
        {hue.BlueLight, new Color(0,140f/255f, 1, 1)},
        {hue.Blue, new Color(0,0,1,1)},
        {hue.Violet, new Color(127f/255f, 0, 1, 1)},
        {hue.Lavender, new Color(238f/255f, 130f/255f, 238f/255f, 1)}
    };


    public enum tile { nadda, room, wall, floor, door }

	public enum direction { north, east, south, west }

    public static Dictionary<tile, GameObject> tiles = new Dictionary<tile, GameObject>()
    {
        {tile.nadda, null},
        {tile.room, null},
        {tile.wall, Resources.Load<GameObject>("tiles/wall")},
        {tile.floor, Resources.Load<GameObject>("tiles/floor")},
        {tile.door, Resources.Load<GameObject>("tiles/door")}
    };


    private static int colorToUse = 0;


    public static int GetNextColor()
    {
        int hackNumHues = Enum.GetNames(typeof(hue)).Length;
        colorToUse++;
        if (colorToUse > hackNumHues - 1){ colorToUse = 0; }
        return colorToUse;
    }

    public const int minRoomSize = 2;

    public const int minRandRooms = 2;
    public const int maxRandRooms = 30;
}
