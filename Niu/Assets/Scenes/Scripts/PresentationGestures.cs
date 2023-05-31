using UnityEngine;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class PresentationGestures : MonoBehaviour
{
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    /*public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint2 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint3 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;*/
    public GameObject OverlayObject;
    public GameObject canvas;
    private ReadingGesture readingGesture;
    public float smoothFactor = 5f;
    private float distanceToCamera = 10f;
    public bool isExecuted = false;
    private int[] coordinates = new int[10];
    private int[,] fields = { { 205, 240, 104, 130}, { 300, 335, 104, 130}, { 395, 430, 104, 130},
                            { 205, 240, 225, 260}, { 300, 335, 225, 260}, { 395, 430, 225, 260},
                            { 205, 240, 345, 390}, { 300, 335, 345, 390}, { 395, 430, 345, 390}};
    private Vector2 posColor;
    private int i = 0;

    // Use this for initialization
    void Start()
    {
        

        readingGesture = Camera.main.GetComponent<ReadingGesture>();
        if (OverlayObject)
        {
            distanceToCamera = (OverlayObject.transform.position - Camera.main.transform.position).magnitude;
        }
        
            canvas.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {

        KinectManager manager = KinectManager.Instance;
        
        if (manager && manager.IsInitialized())
        {
            int iJointIndex = (int)TrackedJoint;
            if (manager.IsUserDetected())
            {
                uint userId = manager.GetPlayer1ID();

                if (manager.IsJointTracked(userId, iJointIndex))
                {
                    Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);
                    if (posJoint != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);


                        // depth pos to color pos
                        posColor = manager.GetColorMapPosForDepthPos(posDepth);

                        float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;
                        Debug.Log(readingGesture.StartDraw);
                        if (isExecuted == false && readingGesture.IsStartDraw())
                        {
                            Draw(posColor);
                        }

                        if (OverlayObject)
                        {
                            Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                            OverlayObject.transform.position = Vector3.Lerp(OverlayObject.transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
                        }
                    }
                }
            }
        }
        if (isExecuted == true)
        {
            for (int j = 0; j < fields.GetLength(0); j++)
            {
                if (posColor.x > fields[j, 0] && posColor.x < fields[j, 1] && posColor.y > fields[j, 2] && posColor.y < fields[j, 3])
                {
                    
                    if (i==1||(j+1 != coordinates[i - 1]))
                    {
                        //Debug.Log(coordinates[i - 1] + " " + j);
                        coordinates[i] = j + 1;
                        i++;
                    }

                }
            }
            if (i>8)
            {
                canvas.SetActive(false);
                isExecuted = false;
            }
            //Debug.Log(string.Join("", new List<int>(coordinates).ConvertAll(i => i.ToString()).ToArray()));
            switch (string.Join("", new List<int>(coordinates).ConvertAll(i => i.ToString()).ToArray()))
            {
                case "0365800000":
                    Debug.Log("Ado");
                    canvas.SetActive(false);
                    isExecuted = false;
                    break;
                case "0351200000":
                    Debug.Log("U\'de");
                    canvas.SetActive(false);
                    isExecuted = false;
                    break;
                case "0321478900":
                    Debug.Log("Eido\'");
                    canvas.SetActive(false);
                    isExecuted = false;
                    break;
                default: 

                    break;
            }
        }
    }

    void Draw(Vector2 posColor)
    {
        if (isExecuted == false)
        {
            
            canvas.SetActive(true);
            i = 1;
            foreach (int k in coordinates)
            {
                coordinates[k] = 0;
            }
            isExecuted = true;
        }
    }
}
