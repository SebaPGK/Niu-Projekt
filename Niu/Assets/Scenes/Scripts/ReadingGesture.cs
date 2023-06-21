using UnityEngine;
using System.Collections;
using System;

public class ReadingGesture : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    public GUIText GestureInfo;

    public bool Psi;
    public bool Tpose;
    public bool StartDraw;
    public bool isDrawing;


    public bool IsPsi()
    {
        if (Psi)
        {
            Psi = false;
            return true;
        }

        return false;
    }

    public bool IsTpose()
    {
        if (Tpose)
        {
            Tpose = false;
            return true;
        }

        return false;
    }
    public bool IsStartDraw() {
        if (StartDraw)
        {
            StartDraw = false;
            return true;
        }

        return false;
    }
    
    

    public void UserDetected(uint userId, int userIndex)
    {
        // detect these user specific gestures
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.Psi);
        manager.DetectGesture(userId, KinectGestures.Gestures.Tpose);
        manager.DetectGesture(userId, KinectGestures.Gestures.StartDraw);
        if (GestureInfo != null)
        {
            GestureInfo.GetComponent<GUIText>().text = "";
        }
    }

    public void UserLost(uint userId, int userIndex)
    {
        if (GestureInfo != null)
        {
            GestureInfo.GetComponent<GUIText>().text = "ni ma nikoigo";
        }
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        // don't do anything here
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        string sGestureText = gesture + " detected";
        if (GestureInfo != null)
        {
            GestureInfo.GetComponent<GUIText>().text = sGestureText;
        }


        if (gesture == KinectGestures.Gestures.Psi)
            Psi = true;
        else if (gesture == KinectGestures.Gestures.Tpose)
            Tpose = true;
        else if (gesture == KinectGestures.Gestures.StartDraw)
            if (isDrawing == false)
            {
                StartDraw = true;
            }

        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        // don't do anything here, just reset the gesture state
        return true;
    }
}
