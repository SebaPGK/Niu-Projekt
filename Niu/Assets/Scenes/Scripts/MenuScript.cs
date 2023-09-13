using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public AudioSource ButtonClick;

	// Use this for initialization
	public void OnPlayButton () {
        ButtonClick.Play();
		SceneManager.LoadScene(1);
    }

    public void OnOptionsButton()
    {
        
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Game")
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
