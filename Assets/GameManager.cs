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
[System.Serializable]
public struct Activity
{
    public string name;
    public int startH;
    public int startM;
    public int dueH;
    public int dueM;
    public int durationM;
    public int durationH;
    //public string location; just in case 
}
public class GameManager : MonoBehaviour
{
    public GameObject timeShow;
    public int currentHour = 6;
    public int currentMinute = 0;
    public static GameManager singleton;
    public Activity sampleGoal;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        showTime();
    }
    //Prototype evaluation func
    public void Eval (string activityName){
        if (activityName.Equals(sampleGoal.name))
        {
            Debug.Log("This sample is ok !");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeTime(int hour,int minutes)
    {
       
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
