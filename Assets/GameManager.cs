using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct EdgeWeight
{
    public Location destination;
    public int hCost;
    public int mCost;
}
public class GameManager : MonoBehaviour
{
    public GameObject timeShow;
    public int currentHour = 6;
    public int currentMinute = 0;
    public static GameManager singleton;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }        
    }
    // Start is called before the first frame update
    void Start()
    {
        showTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeTime(int hour,int minutes)
    {
        Debug.Log("You all??");
        currentHour += hour;
        currentMinute += minutes;
        showTime();
       
        
    }
    void showTime()
    {
        if (currentMinute >= 60)
        {
            currentHour += 1;
            currentMinute = currentMinute - 60;
        }
        if (currentHour < 10)
        {
            timeShow.GetComponent<TextMeshProUGUI>().text = "0" + currentHour;
        }
        else
        {
            timeShow.GetComponent<TextMeshProUGUI>().text =  ""+currentHour;
        }
        if (currentMinute < 10)
        {
            timeShow.GetComponent<TextMeshProUGUI>().text = timeShow.GetComponent<TextMeshProUGUI>().text + ":0" + currentMinute;
        }
        else
        {
            timeShow.GetComponent<TextMeshProUGUI>().text = timeShow.GetComponent<TextMeshProUGUI>().text + ":" + currentMinute;
        }
    }
}
