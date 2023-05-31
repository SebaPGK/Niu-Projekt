using UnityEngine;
using System.Timers;
using System.Collections;
using System.Collections.Generic;

public class PresentationGestures : MonoBehaviour
{
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint2 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint3 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public GameObject OverlayObject;
    public Canvas canvas;
    private ReadingGesture readingGesture;
    public float smoothFactor = 5f;
    private float distanceToCamera = 10f;
    public bool isExecuted = false;
    public int[] coordinates = new int[9];

    // Use this for initialization
    void Start()
    {
        //Cursor.visible = false;
        
        readingGesture = Camera.main.GetComponent<ReadingGesture>();
        if (OverlayObject)
        {
            distanceToCamera = (OverlayObject.transform.position - Camera.main.transform.position).magnitude;
        }
        if (canvas)
        {
            canvas.visiblity
        }
    }

    // Update is called once per frame
    void Update()
    {
        KinectManager manager = KinectManager.Instance;
        //Debug.Log(readingGesture.StartDraw);
        if (manager && manager.IsInitialized())
        {
            int iJointIndex = (int)TrackedJoint;
            if (manager.IsUserDetected())
            {
                uint userId = manager.GetPlayer1ID();

                if (manager.IsJointTracked(userId, iJointIndex))
                {
                    Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);
                    if (readingGesture.StartDraw == true)
                    {
                        Draw(posJoint);
                    }
                    if (posJoint != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);


                        // depth pos to color pos
                        Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);

                        float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;

                        if (readingGesture.StartDraw == true)
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
    }

    void Draw(Vector2 posColor)
    {
        if (isExecuted = !true)
        {
            int i = 0;
            coordinates[i]= 3;
            isExecuted = true;
            i++;
            while (isExecuted == true)
            {
                Debug.Log(posColor);
            /*
                if (posColor.x > 0 && posColor.x< 3 && posColor.y> 0 && posColor.y< 3)
                {
                  if(coordinates[i]== coordinates[i-1])
                    {
                        continue;
                    }
                    coordinates[i] = 1;
                    i++;
                }
                else if (posColor.x > 3 && posColor.x < 6 && posColor.y > 3 && posColor.y < 6)
                {
                    if (coordinates[i] == coordinates[i - 1])
                    {
                        continue;
                    }
                    coordinates[i] = 2;
                    i++;
                }
                else 
                {
                    continue;
                }*/


            }
        }
    }
}
