using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresentationGestures : MonoBehaviour {
    
    private ReadingGesture readingGesture;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        
        readingGesture = Camera.main.GetComponent<ReadingGesture>();
    }
	
	// Update is called once per frame
	void Update () {
        KinectManager kinectManager = KinectManager.Instance;
        Debug.Log("weee");
    }
}
