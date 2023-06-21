using UnityEngine;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine.SceneManagement;

public class PresentationGestures : MonoBehaviour
{
    public GUIText GameInfo;
    public GUIText GameInfo1;
    public GUIText TimeRemainingText;
    private string[] Symbol = {"Ado", "U'de", "Eido'" };
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    /*public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint2 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint3 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;*/
    private float scaleX;
    private float scaleY;
    public GameObject OverlayObject;
    public GameObject canvas;
    public GameObject[] Sprites;
    public GameObject[] Head;
    public GameObject[] Body;
    private SkinnedMeshRenderer HeadChosen;
    private SkinnedMeshRenderer BodyChosen;
    private ReadingGesture readingGesture;
    public float smoothFactor = 5f;
    private float distanceToCamera = 10f;
    public bool isExecuted = false;
    private int[] coordinates = new int[10];
    private float[,] fields = { { 0.36f,0.4f,0.64f,0.68f}, { 0.48f,0.52f,0.64f,0.68f}, { 0.6f,0.64f,0.64f,0.68f},
                            { 0.36f,0.4f,0.48f,0.52f}, { 0.48f,0.52f,0.48f,0.52f}, { 0.6f,0.64f,0.48f,0.52f},
                            { 0.36f,0.4f,0.32f,0.36f}, { 0.48f,0.52f,0.32f,0.36f}, { 0.6f,0.64f,0.32f,0.36f}};
    private Vector2 posColor;
    private int i = 0;
    private int[] randomGolem= new int[2];

    private int[] built = new int[2];
    static int points = 0;
    private float timeRemaining = 120;
    // Use this for initialization
    void Start()
    {
        

        readingGesture = Camera.main.GetComponent<ReadingGesture>();
        if (OverlayObject)
        {
            distanceToCamera = (OverlayObject.transform.position - Camera.main.transform.position).magnitude;
        }
        
            canvas.SetActive(false);
        foreach (GameObject sprites in Sprites)
        {
            sprites.SetActive(false); ;
        }

        foreach (GameObject head in Head)
        {
            head.transform.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        foreach (GameObject body in Body)
        {
            body.transform.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        OverlayObject.GetComponent<MeshRenderer>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        GameInfo1.GetComponent<GUIText>().text ="Punkty: " + points.ToString();
        KinectManager manager = KinectManager.Instance;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
        {
            PlayerPrefs.SetInt("points", points);
            Destroy(gameObject);
            SceneManager.LoadScene(2);

        }
        var ts = TimeSpan.FromSeconds(timeRemaining);
        TimeRemainingText.GetComponent<GUIText>().text = "Time: " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

        if (Input.GetKey("escape"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(2);
        }

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

                        scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;
                        Debug.Log(readingGesture.StartDraw);
                        if (isExecuted == false && readingGesture.IsStartDraw())
                        {
                            Draw(scaleX, scaleY);
                            readingGesture.isDrawing = true;
                        }

                        if (readingGesture.IsTpose())
                        {
                            canvas.SetActive(false);
                            isExecuted = false;
                            readingGesture.isDrawing = false;
                            Debug.Log("Anulowano");
                            OverlayObject.GetComponent<MeshRenderer>().enabled = false;
                            if (HeadChosen != null)
                            {
                                HeadChosen.enabled = false;
                                HeadChosen = null;
                            }
                            if (BodyChosen != null)
                            {
                                BodyChosen.enabled = false;
                                HeadChosen = null;
                            }
                            built[0] = 0; 
                            built[1] = 0;
                            for (int k = 0; k < coordinates.Length; k++)
                            {
                                coordinates[k] = 0;
                            }
                        }

                        if (isExecuted == false && readingGesture.IsPsi() )
                        {
                            Debug.Log(randomGolem[0]+" "+ built[0]+"||||"+ randomGolem[1] + " " + built[1]);
                            if (randomGolem[0] == built[0] && randomGolem[1] == built[1])
                            {
                                points += 5;
                                
                            }
                            else if (built[0] == 0 && built[1] == 0)
                            {
                                //To ma pozostać puste
                            }
                            else
                            {
                                points -= 3;
                            }
                            randomGolem[0] = 0;
                            randomGolem[1] = 0;
                            built[0] = 0;
                            built[1] = 0;
                            OverlayObject.GetComponent<MeshRenderer>().enabled = false;
                            if (HeadChosen != null)
                            {
                                HeadChosen.enabled = false;
                                HeadChosen = null;
                            }
                            if (BodyChosen != null)
                            {
                                BodyChosen.enabled = false;
                                HeadChosen = null;
                            }
                            for (int k = 0; k < coordinates.Length; k++)
                            {
                                coordinates[k] = 0;
                            }
                            Debug.Log("zatwierdzono");
                        }
                        //Debug.Log(scaleX+","+ scaleY);
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
                if (scaleX > fields[j, 0] && scaleX < fields[j, 1] && scaleY > fields[j, 2] && scaleY < fields[j, 3])
                {
                    
                    if (i==1||(j+1 != coordinates[i - 1]))
                    {
                        //Debug.Log(coordinates[i - 1] + " " + j);
                        coordinates[i] = j + 1;
                        i++;
                        Sprites[j].SetActive(true);
                    }

                }
            }
            if (i>8)
            {
                canvas.SetActive(false);
                isExecuted = false;
                readingGesture.isDrawing = false;
                OverlayObject.GetComponent<MeshRenderer>().enabled = false;
                Debug.Log("Brak poprawnego gestu");
                
            }
            //Debug.Log(string.Join("", new List<int>(coordinates).ConvertAll(i => i.ToString()).ToArray()));
            switch (string.Join("", new List<int>(coordinates).ConvertAll(i => i.ToString()).ToArray()))
            {
                case "0365800000":
                    Debug.Log("Ado");
                    Construvt("Ado");
                    canvas.SetActive(false);
                    isExecuted = false;
                    OverlayObject.GetComponent<MeshRenderer>().enabled = false;
                    break;
                case "0351200000":
                    Debug.Log("U\'de");
                    Construvt("U\'de");
                    canvas.SetActive(false);
                    isExecuted = false;
                    OverlayObject.GetComponent<MeshRenderer>().enabled = false;
                    break;
                case "0321478900":
                    Debug.Log("Eido\'");
                    Construvt("Eido\'");
                    canvas.SetActive(false);
                    isExecuted = false;
                    OverlayObject.GetComponent<MeshRenderer>().enabled = false;
                    break;
                default:
                    
                    break;
            }
        }

        if (randomGolem[0] == 0 && randomGolem[1] == 0)
        {
            AssignRandomGolem();
        }
        
    }

    void Draw(float scaleX, float scaleY)
    {
        if (isExecuted == false)
        {
            foreach (GameObject sprites in Sprites)
            {
                sprites.SetActive(false); ;
            }
            OverlayObject.GetComponent<MeshRenderer>().enabled = true;
            canvas.SetActive(true);
            i = 1;
            
            for (int k = 0; k < coordinates.Length; k++)
            {
                coordinates[k] = 0;
            }
            isExecuted = true;
        }
    }

    void Construvt(string element) { 
        switch (element)
        {
            case "Ado":
               
                
                if (built[0] != 0) { 
                    built[1] = 1;
                    HeadChosen = Head[0].transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                    HeadChosen.enabled = true;
                } else { 
                    built[0] = 1;
                    BodyChosen = Body[0].transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                    BodyChosen.enabled = true;
                }
                break;
            case "U\'de":
                
                
                if(built[0] != 0) {
                    built[1] = 2;
                    HeadChosen = Head[1].transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                    HeadChosen.enabled = true;
                } else
                {
                    built[0] = 2;
                    BodyChosen = Body[1].transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                    BodyChosen.enabled = true;
                }
                break;
            case "Eido\'":
               
                
                if (built[0] != 0) { 
                    built[1] = 3;
                    HeadChosen = Head[2].transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                    HeadChosen.enabled = true;
                } else { 
                    built[0] = 3;
                    BodyChosen = Body[2].transform.gameObject.GetComponent<SkinnedMeshRenderer>();
                    BodyChosen.enabled = true;
                }
                break;
        }
        readingGesture.isDrawing = false;

    }
    void AssignRandomGolem()
    {
        randomGolem[0] = UnityEngine.Random.Range(1, 4);
        randomGolem[1] = UnityEngine.Random.Range(1, 4);

        GameInfo.GetComponent<GUIText>().text ="For Body: " + Symbol[randomGolem[0]-1] +"\nFor Head: "+ Symbol[randomGolem[1] - 1];
    }


}
