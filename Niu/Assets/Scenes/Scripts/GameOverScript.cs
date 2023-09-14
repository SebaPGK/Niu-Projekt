using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {
    int points;
    public Text GameInfo;
    public AudioSource ButtonClick;
    // Use this for initialization
    public void OnRestartButton () {
        ButtonClick.Play();
        SceneManager.LoadScene(0);
	}

    public void OnQuitButton()
    {
        Application.Quit();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        points = PlayerPrefs.GetInt("points");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("escape"))
        {
			Application.Quit();
        }
        
        GameInfo.text = "You got " + points + " points";

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Menu")
        {
            Canvas canvas = gameObject.GetComponent<Canvas>();
            canvas.enabled = false;

            if (ButtonClick.isPlaying == false)
            {
                Destroy(gameObject);
            }
        }

    }
}
