using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Location : MonoBehaviour, IPointerClickHandler
{
    public PlayerMovement player;
    public EdgeWeight[] neighbours;
    public bool canMove;
    public int timeCostH = 0;
    public int timeCostM = 0;
    public Activity[] activities;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canMove)
        {
            player.ChangeLocation(this);
            if (activities != null)
                GameManager.singleton.Eval(activities[0].name);
        }
        
    }
    public void BroadCastCanMove()
    {
        foreach (EdgeWeight n in neighbours)
        {
            n.destination.canMove = true;
        }
    }
    public void BroadCastCanTMove()
    {
        foreach (EdgeWeight n in neighbours)
        {
            n.destination.canMove = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
     * 
     */

   
}
