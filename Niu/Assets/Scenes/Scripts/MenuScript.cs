using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public AudioSource ButtonClick;
    public Canvas HowToPlay;

    public Canvas[] slides;
    private int slideId = 0;

	// Use this for initialization
	public void OnPlayButton () {
        ButtonClick.Play();
		SceneManager.LoadScene(1);
    }

    public void OnHowToPlayButton()
    {
        ButtonClick.Play();
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;

        Canvas howTo = HowToPlay.GetComponent<Canvas>();
        howTo.enabled = true;

        CanvasGroup howToGroup = HowToPlay.GetComponent<CanvasGroup>();
        howToGroup.interactable = true;
        howToGroup.blocksRaycasts = true;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnMenuButton()
    {
        ButtonClick.Play();
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = true;

        Canvas howTo = HowToPlay.GetComponent<Canvas>();
        howTo.enabled = false;

        CanvasGroup howToGroup = HowToPlay.GetComponent<CanvasGroup>();
        howToGroup.interactable = false;
        howToGroup.blocksRaycasts = false;
    }

    public void NextSlide()
    {
        Canvas previousSlide = slides[slideId].GetComponent<Canvas>();
        previousSlide.enabled = false;

        if (slideId == slides.Length - 1)
        {
            slideId = 0;
        } else
        {
            slideId++;
        }

        Canvas thisSlide = slides[slideId].GetComponent<Canvas>();
        thisSlide.enabled = true;
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
