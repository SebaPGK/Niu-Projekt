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
    private ReadingGesture readingGesture;
    public float smoothFactor = 5f; 
    private float distanceToCamera = 10f;

    // Use this for initialization
    void Start()
    {
        //Cursor.visible = false;

        readingGesture = Camera.main.GetComponent<ReadingGesture>();
        if (OverlayObject)
        {
            distanceToCamera = (OverlayObject.transform.position - Camera.main.transform.position).magnitude;
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
                        Draw();
                    }
                    if (posJoint != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);
                        

                        // depth pos to color pos
                        Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);

                        float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;



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

    void Draw()
    {

    }
}
