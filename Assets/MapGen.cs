
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapGen : MonoBehaviour {

	[System.Serializable]
	public struct Activity {
		[SerializeField]
		public string name;
		[SerializeField]
		public int start;
		[SerializeField]
		public int due;
		[SerializeField]
		public int duration;
		[SerializeField]
		public string locationName;
	}


    public SpriteHolder sh;
	// Use this for initialization
	[SerializeField]
	public List<Activity> activityList;
	public List<string[]> itenary;
	public List<Itenary> dayItenaries;
	public int mapSize;//map assummes square for ease of generation and UI
	public LocationNode[,] map;
	List<Activity> activityCounter = new List<Activity>();
    public int[,] test;
    //Debugging purposes
    //Preprocessing in map gen var
    public List<int> travelPair = new List<int>();
    public List<string> locationPair1 = new List<string>();
    public List<string> locationPair2 = new List<string>();
    public GameObject buildingAnchor;
    public GameObject roadAnchor;
    //
    public List<LocationNode> finalLocationLists;
    void Start() {
		dayItenaries = new List<Itenary>();
		int workTravelTime = Random.Range(1, 4);
		Itenary day1 = new Itenary();
		day1.activities = new List<Activity>();
		day1.activities.Add(activityList[0]);
		day1.activities.Add(activityList[1]);
		day1.activities.Add(activityList[2]);
		day1.travelTimes = new List<int>();
		day1.travelTimes.Add(workTravelTime);
		day1.travelTimes.Add(Random.Range(1, 4));
		day1.travelTimes.Add(Random.Range(1, 4));
        
		Itenary day2 = new Itenary();
		day2.activities = new List<Activity>();
		day2.activities.Add(activityList[0]);
		day2.activities.Add(activityList[3]);
		day2.activities.Add(activityList[4]);
		day2.travelTimes = new List<int>();
		day2.travelTimes.Add(workTravelTime);
		day2.travelTimes.Add(Random.Range(1, 4));
		day2.travelTimes.Add(Random.Range(1, 4));


		dayItenaries.Add(day1);
		dayItenaries.Add(day2);
		map = new LocationNode[mapSize, mapSize];
        MapGenerator();
	}

	// Update is called once per frame
	void Update() {

	}
	//itenary generator
	void ItenaryGenerator()
	{

	}
	void MapGenerator()
	{
		activityCounter = new List<Activity>(activityList);
		List<LocationNode> placedLocations;
        List<PlacementData> placementCandidates;//used to list possible places to spawn the building
        List<Cleansing> cleansingData = new List<Cleansing>();
        int fillerCounter = 0;

        for (int i = 0; i < mapSize; i++)
		{
			for (int j = 0; j < mapSize; j++)
			{
				map[i, j] = new LocationNode();
                map[i, j].state = LocationNode.EMPTY;
                map[i, j].x = i;
                map[i, j].y = j;
			}
		}

		int center = Mathf.CeilToInt(mapSize / 2);
		map[center, center].name = "Home";
        map[center, center].state = LocationNode.FILLED;
       // Debug.Log("Part1");
        //Part 1 preprocessing
        locationPair1.Add("Home");
        locationPair2.Add("Office");
        travelPair.Add(dayItenaries[0].travelTimes[0]);
        //Debug.Log(travelPair.Count);
        foreach (Itenary itenary in dayItenaries)
        {
            int index = 0;
           
            int pair1Exist = -1;
            int pair2Exist = -1;
     
            while(index < itenary.activities.Count-1)
            {
                //i the activity as first member of pair is not found in actipair 1 put it in
                for(int i = 0; i < locationPair1.Count; i++)
                {
                    if(locationPair1[i] == itenary.activities[index].locationName)
                    {
                        pair1Exist = i;
                        break;
                    }
                }
                for (int k = 0; k < locationPair1.Count; k++)
                {
                    if (locationPair2[k] == itenary.activities[index+1].locationName)
                    {
                        pair2Exist = k;
                        break;
                    }
                }
                //pair1Exist = locationPair1.FindIndex(u => u == itenary.activities[index].locationName);
               // Debug.Log(pair1Exist);
               // pair2Exist = locationPair2.FindIndex(u => u == itenary.activities[index+1].locationName);
               if(pair1Exist >= 0)
                {
                    if(pair2Exist < 0)
                    {
                        locationPair1.Add(itenary.activities[index].locationName);
                        locationPair2.Add(itenary.activities[index + 1].locationName);
                        travelPair.Add(itenary.travelTimes[index+1]);
                    }
                   
                }else
                {
                    locationPair1.Add(itenary.activities[index].locationName);
                    locationPair2.Add(itenary.activities[index + 1].locationName);
                    travelPair.Add(itenary.travelTimes[index + 1]);
                }
              
                index++;
                pair1Exist = -1;
                pair2Exist = -1;
            }
			
        }
        ///Part 2 Arranging
        ///i=1 because first location is home and already placed beforehand
        //Debug.Log("Part2");
        placedLocations = new List<LocationNode>();
        LocationNode currentInspected = map[center, center];//home\
        currentInspected.name =locationPair1[0];
        map[center, center].state = LocationNode.FILLED;
        placedLocations.Add(currentInspected);

        List<Vector2> coorPairs;
        int travelDist;
        for(int i = 0; i < locationPair1.Count; i++)
        {
            //GeneratingCoorPairs
            int counter = 0;
            travelDist = travelPair[i];
            coorPairs = new List<Vector2>();
            placementCandidates = new List<PlacementData>();
            while (counter <= travelDist)
            {
                coorPairs.Add(new Vector2(counter, travelDist - counter));
                if (counter != 0)
                {
                    coorPairs.Add(new Vector2(-counter, travelDist - counter));
                    
                }
                if ((travelDist-counter != 0))
                {
                    
                    coorPairs.Add(new Vector2(counter, -(travelDist - counter)));
                }
                if(counter != 0 && (travelDist - counter != 0))
                {
                    coorPairs.Add(new Vector2(-counter, -(travelDist - counter)));
                }
               
                //Debug.Log(counter + " " + (travelDist - counter));
                counter++;
            }
            foreach(Vector2 v in coorPairs)
            {
                if(currentInspected.x + (int)v.x >= mapSize || currentInspected.x + (int)v.x < 0 ||
                    currentInspected.y + (int)v.y >= mapSize || currentInspected.y + (int)v.y < 0)
                {
                    continue;
                }
                if (map[currentInspected.x + (int)v.x, currentInspected.y + (int)v.y].state == LocationNode.EMPTY ||
                   map[currentInspected.x + (int)v.x, currentInspected.y + (int)v.y].state == LocationNode.ROAD)
                {
                    PlacementData x = new PlacementData();
                    x.op = v;
                    x.targetNode = map[currentInspected.x + (int)v.x, currentInspected.y + (int)v.y];
                    placementCandidates.Add(x);
                }

            }
            int rand = Random.Range(0, placementCandidates.Count);
            PlacementData selected = placementCandidates[rand];

            int r = Random.Range(0, 2);
            int addingX = Mathf.Abs((int)selected.op.x);
            int addingY = Mathf.Abs((int)selected.op.y);
            map[currentInspected.x + (int)selected.op.x, currentInspected.y + (int)selected.op.y].state = LocationNode.FILLED;
            map[currentInspected.x + (int)selected.op.x, currentInspected.y + (int)selected.op.y].name = locationPair2[i];
            bool found = false;
            foreach(LocationNode l in placedLocations)
            {
                if(l.name == locationPair2[i])
                {
                    found = true;
                }
            }
            if (!found)
            {
                placedLocations.Add(map[currentInspected.x + (int)selected.op.x, currentInspected.y + (int)selected.op.y]);
            }
            if (r == 1)
            {
               for(int k = 0; k < addingX;k++)
                {
                    if (map[currentInspected.x + (((int)selected.op.x / (int)addingX))*(k+1), currentInspected.y].state == LocationNode.EMPTY ||
                        map[currentInspected.x + (((int)selected.op.x / (int)addingX)) * (k + 1), currentInspected.y].state == LocationNode.ROAD)
                    {
                        map[currentInspected.x + (((int)selected.op.x / (int)addingX)) * (k + 1), currentInspected.y].state = LocationNode.ROAD;
                        map[currentInspected.x + (((int)selected.op.x / (int)addingX)) * (k + 1), currentInspected.y].tags.Add(i);
                    }
                }
                //Debug.Log(map[currentInspected.x + (int)selected.op.x, currentInspected.y].name + " name");
                if (map[currentInspected.x + (int)selected.op.x, currentInspected.y].state == LocationNode.ROAD ||
                    map[currentInspected.x + (int)selected.op.x, currentInspected.y].state == LocationNode.EMPTY)
                {
                    map[currentInspected.x + (int)selected.op.x, currentInspected.y].state = LocationNode.FILLER;
                    map[currentInspected.x + (int)selected.op.x, currentInspected.y].name = "Other Building " + fillerCounter;
                    Debug.Log(map[currentInspected.x + (int)selected.op.x, currentInspected.y].name);
                    fillerCounter++;
                }
               
               for (int l = 0; l < addingY; l++)
                {
                    if (map[currentInspected.x + (int)selected.op.x, currentInspected.y+ (((int)selected.op.y / (int)addingY)) * (l + 1)].state == LocationNode.ROAD ||
                        map[currentInspected.x + (int)selected.op.x, currentInspected.y + (((int)selected.op.y / (int)addingY)) * (l + 1)].state == LocationNode.EMPTY)
                    {
                        map[currentInspected.x + (int)selected.op.x, currentInspected.y + (((int)selected.op.y / (int)addingY)) * (l + 1)].state = LocationNode.ROAD;
                        map[currentInspected.x + (int)selected.op.x, currentInspected.y + (((int)selected.op.y / (int)addingY)) * (l + 1)].tags.Add(i);
                    }
                }
                    
            }else if(r == 0)
            {
              
                for (int l = 0; l < addingY; l++)
                {
                    if (map[currentInspected.x, currentInspected.y + (((int)selected.op.y / (int)addingY)) * (l + 1)].state == LocationNode.EMPTY ||
                        map[currentInspected.x, currentInspected.y + (((int)selected.op.y / (int)addingY)) * (l + 1)].state == LocationNode.ROAD)
                    {
                        map[currentInspected.x , currentInspected.y + (((int)selected.op.y / (int)addingY)) * (l + 1)].state = LocationNode.ROAD;
                        map[currentInspected.x, currentInspected.y + (((int)selected.op.y / (int)addingY)) * (l + 1)].tags.Add(i);
                    }
                }
                //Debug.Log(map[currentInspected.x, currentInspected.y + (int)selected.op.y].name + " name");
                if(map[currentInspected.x, currentInspected.y + (int)selected.op.y].state == LocationNode.EMPTY ||
                   map[currentInspected.x, currentInspected.y + (int)selected.op.y].state == LocationNode.ROAD)
                {
                    Debug.Log(map[currentInspected.x, currentInspected.y + (int)selected.op.y].name);
                    map[currentInspected.x, currentInspected.y + (int)selected.op.y].state = LocationNode.FILLER;
                    map[currentInspected.x, currentInspected.y + (int)selected.op.y].name = "Other Building " + fillerCounter;
                    Debug.Log(map[currentInspected.x, currentInspected.y + (int)selected.op.y].name);
                    fillerCounter++;
                }
               
                
                for (int k = 0; k < addingX; k++)
                {
                    if (map[currentInspected.x + (((int)selected.op.x / (int)addingX)) * (k + 1), currentInspected.y+(int)selected.op.y].state == LocationNode.ROAD ||
                        map[currentInspected.x + (((int)selected.op.x / (int)addingX)) * (k + 1), currentInspected.y + (int)selected.op.y].state == LocationNode.EMPTY)
                    {
                        map[currentInspected.x + (((int)selected.op.x / (int)addingX)) * (k + 1), currentInspected.y + (int)selected.op.y].state = LocationNode.ROAD;
                        map[currentInspected.x + (((int)selected.op.x / (int)addingX)) * (k + 1), currentInspected.y + (int)selected.op.y].tags.Add(i);
                    }
                }

            }
            Cleansing c = new Cleansing();
            c.source = currentInspected;
            c.destination = map[currentInspected.x + (int)selected.op.x, currentInspected.y + (int)selected.op.y];
            c.op = selected.op;
            c.isTracedVerticalFirst = (r == 0) ? true : false;
            c.tag = i;
            cleansingData.Add(c);
            if(i +1 >= locationPair1.Count)
            {
                continue;
            }
            foreach (LocationNode l in placedLocations)
            {
                if (l.name == locationPair1[i+1])
                {
                   
                    currentInspected = l;
                    break;
                }
            }
            
           // currentInspected = map[currentInspected.x + (int)selected.op.x, currentInspected.y + (int)selected.op.y];
        }

       
        //Debug.Log("Part3");
        ///Part 3 Cleaning
        ///adding Filler if there is more than 1 tag in map and it is not filled with building
        for (int i = 0; i < mapSize; i++)
        {
            
            for (int j = 0; j < mapSize; j++)
            {

                if (map[i, j].tags.Count > 1 && map[i,j].state == LocationNode.ROAD)
                {
                    fillerCounter++;
                    map[i, j].state = LocationNode.FILLER;
                    map[i, j].name = "filler "+fillerCounter;
                    Debug.Log(map[i, j].name);
                }
            }
        }
        int debugCounter = 0;
        Debug.Log(cleansingData.Count);
        foreach(Cleansing c in cleansingData)
        {
            LocationNode source = c.source;
            LocationNode destination = c.destination;
            int sourceX = c.source.x;
            int sourceY = c.source.y;
            bool verticalSimplification = true;
            int verticalSimpCounter = 0;
            int horizontalSimpCounter = 0;
            bool horizontalSimplification = true;
            LocationNode checkedNode = source;
            int traceX = Mathf.Abs((int)c.op.x);
            int traceY = Mathf.Abs((int)c.op.y);
            
            //Debug.Log(c.source.name);
            Debug.Log("Checking "+ c.source.name + " "+c.destination.name+ " "+ debugCounter);
            if (c.isTracedVerticalFirst)
            {
                
                int multiplier = 1;
                if(traceY == 0)
                {
                    multiplier = 1;
                }else
                {
                    multiplier = ((int)c.op.y / traceY);
                }
               
                for (int k = 0; k < traceY; k++)
                {
                    
                    if (map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].state == LocationNode.ROAD)
                    {
                        verticalSimpCounter++;
                        
                    }else if(map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].state == LocationNode.FILLER ||
                        map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].state == LocationNode.FILLED)
                    {

                        verticalSimpCounter++;
                        Debug.Log("Vertical First, Vertical check");
                        Debug.Log(checkedNode.name);
                        Debug.Log(map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].name);
                        
                        ////Processing neighbourhood
                        if(multiplier < 0)
                        {
                            checkedNode.downNeighbour = map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            checkedNode.downNeighbourName = map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].name;
                            checkedNode.downNeighbourWeight = verticalSimpCounter;
                            map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].upNeighbour = checkedNode;
                            map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].upNeighbourName = checkedNode.name;
                            map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].upNeighbourWeight = verticalSimpCounter;
                            checkedNode = map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            verticalSimpCounter = 0;
                        }else if(multiplier >= 0)
                        {
                            checkedNode.upNeighbour = map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            checkedNode.upNeighbourName = map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].name;
                            checkedNode.upNeighbourWeight = verticalSimpCounter;
                            map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].downNeighbour = checkedNode;
                            map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].downNeighbourName = checkedNode.name;
                            map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)].downNeighbourWeight = verticalSimpCounter;
                            checkedNode = map[sourceX, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            verticalSimpCounter = 0;
                        }
                    }
                }
                //horizontalTrace
                if(traceX == 0)
                {
                    multiplier = 1;
                }else
                {
                    multiplier = ((int)c.op.x / traceX);
                }
               
                for (int j = 0; j < traceX; j++)
                {
                    if (map[sourceX+((j + 1) * (int)c.op.x / traceX), sourceY +(int)c.op.y].state == LocationNode.ROAD)
                    {
                        horizontalSimpCounter++;
                    }else if(map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].state == LocationNode.FILLER ||
                        map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].state == LocationNode.FILLED)
                    {
                        horizontalSimpCounter++;
                        Debug.Log("Vertical First, Horizontal check");
                        Debug.Log(checkedNode.name);
                        Debug.Log(map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].name);
                        if (multiplier < 0)
                        {
                            checkedNode.leftNeighbour = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y];
                            checkedNode.leftNeighbourName = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].name;
                            checkedNode.leftNeighbourWeight = horizontalSimpCounter;
                            
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].rightNeighbour = checkedNode;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].rightNeighbourName = checkedNode.name;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].rightNeighbourWeight = horizontalSimpCounter;
                            horizontalSimpCounter = 0;
                            checkedNode = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y];
                        }else if(multiplier >= 0)
                        {
                            checkedNode.rightNeighbour = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y];
                            checkedNode.rightNeighbourName = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].name;
                            checkedNode.rightNeighbourWeight = horizontalSimpCounter;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].leftNeighbour = checkedNode;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].leftNeighbourName = checkedNode.name;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y].leftNeighbourWeight = horizontalSimpCounter;
                            horizontalSimpCounter = 0;
                            checkedNode = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY + (int)c.op.y];
                        }
                    }
                }
            }else if (!c.isTracedVerticalFirst)
            {


                //horizontalTrace
                int multiplier;
                if (traceX != 0)
                {
                    multiplier = ((int)c.op.x / traceX);
                }else
                {
                    multiplier = 1;
                }
                for (int j = 0; j < traceX; j++)
                {
                    if (map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].state == LocationNode.ROAD)
                    {
                        horizontalSimpCounter++;
                    }
                    else if (map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY ].state == LocationNode.FILLER ||
                       map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].state == LocationNode.FILLED)
                    {
                        Debug.Log("Horizontal First, Horizontal check");
                        Debug.Log(checkedNode.name);
                        Debug.Log(map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].name);
                        horizontalSimpCounter++;
                        if (multiplier < 0)
                        {
                            checkedNode.leftNeighbour = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY ];
                            checkedNode.leftNeighbourName = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].name;
                            checkedNode.leftNeighbourWeight = horizontalSimpCounter;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY ].rightNeighbour = checkedNode;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].rightNeighbourName = checkedNode.name;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].rightNeighbourWeight = horizontalSimpCounter;
                            horizontalSimpCounter = 0;
                            checkedNode = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY ];
                        }
                        else if (multiplier >= 0)
                        {
                            checkedNode.rightNeighbour = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY];
                            checkedNode.rightNeighbourName = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].name;
                            checkedNode.rightNeighbourWeight = horizontalSimpCounter;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY ].leftNeighbour = checkedNode;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY].leftNeighbourName = checkedNode.name;
                            map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY ].leftNeighbourWeight = horizontalSimpCounter;

                            horizontalSimpCounter = 0;
                            checkedNode = map[sourceX + ((j + 1) * (int)c.op.x / traceX), sourceY ];
                        }
                    }
                }
                //verticalTrace
                if(traceY == 0)
                {
                    multiplier = 1;
                }else
                {
                    multiplier = ((int)c.op.y / traceY);
                }
                
                for (int k = 0; k < traceY; k++)
                {
                    if (map[sourceX+(int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].state == LocationNode.ROAD)
                    {
                        verticalSimpCounter++;
                    }
                    else if (map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].state == LocationNode.FILLER ||
                       map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].state == LocationNode.FILLED)
                    {
                        Debug.Log("Horizontal First, Vertical check");
                        Debug.Log(checkedNode.name);
                        Debug.Log(map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].name);
                        verticalSimpCounter++;
                        ////Processing neighbourhood
                        if (multiplier < 0)
                        {
                            checkedNode.downNeighbour = map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            checkedNode.downNeighbourName = map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].name;
                            checkedNode.downNeighbourWeight = verticalSimpCounter;
                            map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].upNeighbour = checkedNode;
                            map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].upNeighbourName = checkedNode.name;
                            map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].upNeighbourWeight = verticalSimpCounter;
                            checkedNode = map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            verticalSimpCounter = 0;
                        }
                        else if (multiplier >= 0)
                        {
                            checkedNode.upNeighbour = map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            checkedNode.upNeighbourName = map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].name;
                            checkedNode.upNeighbourWeight = verticalSimpCounter;
                            map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].downNeighbour = checkedNode;
                            map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].downNeighbourName = checkedNode.name;
                            map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)].downNeighbourWeight = verticalSimpCounter;
                            checkedNode = map[sourceX + (int)c.op.x, sourceY + ((k + 1) * (int)c.op.y / traceY)];
                            verticalSimpCounter = 0;
                        }
                    }
                }
            }
            Debug.Log("End Checking " + c.source.name + " " + debugCounter);
            debugCounter++;
        }
        ///CH 4 actuating
        ///Listing\
        //Debug.Log("Part4");
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {

                if (map[i, j].state == LocationNode.FILLER || map[i, j].state == LocationNode.FILLED)
                {

                    finalLocationLists.Add(map[i, j]);
                }
            }
        }
        LocationNode[,] newMaps = new LocationNode[mapSize + 2, mapSize + 2];
        for(int i = 0; i < mapSize + 2; i++)
        {
            for(int j = 0; j < mapSize + 2; j++)
            {
                newMaps[i, j] = new LocationNode();
            }
        }
        newMaps[Mathf.CeilToInt((mapSize + 2) / 2), Mathf.CeilToInt((mapSize + 2) / 2)] = map[center, center];
        Stack<LocationNode> genStack = new Stack<LocationNode>();
        LocationNode processed = newMaps[Mathf.CeilToInt((mapSize + 2) / 2), Mathf.CeilToInt((mapSize + 2) / 2)];
        processed.x = Mathf.CeilToInt((mapSize + 2) / 2);
        processed.y = Mathf.CeilToInt((mapSize + 2) / 2);
        Debug.Log(processed.name);
        List<LocationNode> NotPlaced = new List<LocationNode>(finalLocationLists);
        bool AlreadyPlaced = true;


        int blockSize = 80;
        
        int padding = 10;
        while( processed != null)
        {

            //processed.x = Mathf.CeilToInt((mapSize + 2) / 2);
            //processed.y = Mathf.CeilToInt((mapSize + 2) / 2);
            if (!processed.placed)
            {
                Vector3 pos = new Vector3(processed.x * blockSize, processed.y * blockSize);
                GameObject g1 = Instantiate(sh.buildings, pos,sh.buildings.transform.rotation);
                g1.transform.SetParent(buildingAnchor.transform);
                g1.transform.GetChild(0).GetComponent<Text>().text = processed.name;
                processed.placed = true;
            }
            if (processed.downNeighbour != null)
            {
                if (!processed.downNeighbour.placed) {
                Debug.Log("Done");
                genStack.Push(processed.downNeighbour);
                Vector3 pos = new Vector3(processed.x * blockSize, (processed.y - 2) * blockSize);
                    processed.downNeighbour.x = processed.x;
                    processed.downNeighbour.y = processed.y - 2;
                GameObject g1 = Instantiate(sh.buildings, pos,sh.buildings.transform.rotation);
                g1.transform.SetParent(buildingAnchor.transform);
                g1.transform.GetChild(0).GetComponent<Text>().text = processed.downNeighbourName;
                pos = new Vector3(processed.x * blockSize, (processed.y - 1) * blockSize);
                GameObject g2 = Instantiate(sh.verticalRoad, pos, sh.verticalRoad.transform.rotation);
                g2.transform.SetParent(roadAnchor.transform);
                g2.transform.GetChild(0).GetComponent<Text>().text = blockToHour(processed.downNeighbourWeight);
                processed.downNeighbour.placed = true;
                }
            }
            if (processed.upNeighbour != null )
            {
                if(!processed.upNeighbour.placed) { 
                Debug.Log("Done");
                genStack.Push(processed.upNeighbour);
                    processed.upNeighbour.x = processed.x;
                    processed.upNeighbour.y = processed.y + 2;
                    Vector3 pos = new Vector3(processed.x * blockSize, (processed.y + 2) * blockSize);
                GameObject g1 = Instantiate(sh.buildings, pos, sh.buildings.transform.rotation);
                g1.transform.SetParent(buildingAnchor.transform);
                g1.transform.GetChild(0).GetComponent<Text>().text = processed.upNeighbourName;
                pos = new Vector3(processed.x * blockSize, (processed.y + 1) * blockSize);
                GameObject g2 = Instantiate(sh.verticalRoad, pos, sh.verticalRoad.transform.rotation);
                g2.transform.SetParent(roadAnchor.transform);
                g2.transform.GetChild(0).GetComponent<Text>().text = blockToHour(processed.upNeighbourWeight);
                processed.upNeighbour.placed = true;
                }
            }
            if (processed.leftNeighbour != null)
            {
                if (!processed.leftNeighbour.placed) { 
                Debug.Log("Done");
                genStack.Push(processed.leftNeighbour);
                    processed.leftNeighbour.x = processed.x-2;
                    processed.leftNeighbour.y = processed.y;
                Vector3 pos = new Vector3((processed.x-2) * blockSize, (processed.y) * blockSize);
                GameObject g1 = Instantiate(sh.buildings, pos, sh.buildings.transform.rotation);
                g1.transform.SetParent(buildingAnchor.transform);
                g1.transform.GetChild(0).GetComponent<Text>().text = processed.leftNeighbourName;
                pos = new Vector3((processed.x-1) * blockSize, (processed.y) * blockSize);
                GameObject g2 = Instantiate(sh.horizontalRoad, pos, sh.horizontalRoad.transform.rotation);
                g2.transform.SetParent(roadAnchor.transform);
                g2.transform.GetChild(0).GetComponent<Text>().text = blockToHour(processed.leftNeighbourWeight);
                processed.leftNeighbour.placed = true;
                }
            }
            if (processed.rightNeighbour != null)
            {
                if (!processed.rightNeighbour.placed) { 
                Debug.Log("Done");
                genStack.Push(processed.rightNeighbour);
                    processed.rightNeighbour.x = processed.x + 2;
                    processed.rightNeighbour.y = processed.y;
                    Vector3 pos = new Vector3((processed.x + 2) * blockSize, (processed.y) * blockSize);
                GameObject g1 = Instantiate(sh.buildings, pos, sh.buildings.transform.rotation);
                g1.transform.SetParent(buildingAnchor.transform);
                g1.transform.GetChild(0).GetComponent<Text>().text = processed.rightNeighbourName;
                pos = new Vector3((processed.x + 1) * blockSize, (processed.y) * blockSize);
                GameObject g2 = Instantiate(sh.horizontalRoad, pos, sh.horizontalRoad.transform.rotation);
                g2.transform.SetParent(roadAnchor.transform);
                g2.transform.GetChild(0).GetComponent<Text>().text = blockToHour(processed.rightNeighbourWeight);
                processed.rightNeighbour.placed = true;
                }
            }
            if (genStack.Count != 0)
                processed = genStack.Pop();
            else
                processed = null;


        }
        
        


    }
    string blockToHour(int block)
    {
        Debug.Log(block);
        string ret = "";
            int hour = 0;
        if (block%2 == 1)
        {
             hour = (block - 1) / 2;
            ret = hour + "." + "5 jam";

        }else
        {
            hour = block / 2;
            ret = hour + " jam";
        }
        return ret;
    }
	
}
