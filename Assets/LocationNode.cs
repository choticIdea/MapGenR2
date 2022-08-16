using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationNode  {
    public string name;
    public LocationNode upNeighbour;
	public LocationNode downNeighbour;
	public LocationNode rightNeighbour;
	public LocationNode leftNeighbour;
    //DebuggingPurpose
    public string upNeighbourName;
    public string downNeighbourName;
    public string rightNeighbourName;
    public string leftNeighbourName;
    public int upNeighbourWeight;
    public int downNeighbourWeight;
    public int rightNeighbourWeight;
    public int leftNeighbourWeight;
    
	public MapGen.Activity activity;
    public List<int> tags = new List<int>();
    public int state = EMPTY;
    public static int EMPTY = 0;
    public static int FILLED = 1;
    public static int FILLER = 2;
    public static int ROAD = 3;
    public int x;
    public int y;
    public bool placed = false;

}
