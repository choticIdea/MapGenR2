using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isMoving  = false;
    public Vector3 destPost = Vector3.zero;
    public Vector3 movVector = Vector3.zero;
   // public Vector3 mosPoint = Vector3.zero;
    public Vector3 nextLocation = Vector3.zero;
    public Vector3 tr = Vector3.zero;
    public Location currentLocation;
   

    void Start()
    {
        currentLocation.BroadCastCanMove();
    }

    // Update is called once per frame
    void Update()
    {
        /* tr = transform.position;
         if (Input.GetMouseButtonDown(0))
         {
             isMoving = true;
            // RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent.GetComponent<RectTransform>(),Input.mousePosition,Camera.main,out destPost);
             //mosPoint = destPost;
         }
         if (isMoving)
         {
            movVector = nextLocation - this.gameObject.transform.position;
             Debug.Log(Mathf.Abs((nextLocation - transform.position).sqrMagnitude));
             this.gameObject.transform.Translate(movVector.normalized * Time.deltaTime*150);
             if (Mathf.Abs((nextLocation - transform.position).magnitude) < 0.3)
             {

                 gameObject.transform.position = nextLocation;
                 isMoving = false;

             }
         }
        */
        
    }
    public void ChangeLocation(Location nextLoc)
    {
        EdgeWeight road;
        road = new EdgeWeight();
        foreach (EdgeWeight e in nextLoc.neighbours)
        {
            if(e.destination == currentLocation)
            {
                road = e;
            }
        }
        currentLocation.BroadCastCanTMove();
        currentLocation = nextLoc;
        currentLocation.BroadCastCanMove();
        transform.position = nextLoc.transform.position;
        // This snippet
        // calculate travel cost 
        int hour = road.hCost;
        int min = road.mCost;
        if(nextLoc.activities != null)
        {
          
            
            int totalMRef = nextLoc.activities[0].startH * 60 + nextLoc.activities[0].startM;
            int total = (GameManager.singleton.currentHour+hour) * 60 + GameManager.singleton.currentMinute+min;
            if (totalMRef > total)
            {
                min += totalMRef - total;
            }
            Debug.Log(hour + " " + min);
            hour += nextLoc.activities[0].durationH;
            min += nextLoc.activities[0].durationM;
        }

        Debug.Log(hour + " " + min);
        GameManager.singleton.ChangeTime(hour,min);
        //should not be here 
    }
}
