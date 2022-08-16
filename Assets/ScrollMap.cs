using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMap : MonoBehaviour {

	// Use this for initialization
	bool flag;
	Vector3 initialMousePos;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
			flag = true;
			initialMousePos = Input.mousePosition;

        }else if (Input.GetMouseButtonUp(0))
        {
			flag = false;
        }
        if (flag)
        {

        }
	}
	
}
