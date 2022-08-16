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
        GameManager.singleton.ChangeTime(road.hCost,road.mCost);
    }
}
