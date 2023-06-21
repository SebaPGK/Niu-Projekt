using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {
    int points;
    public GUIText GameInfo;
    // Use this for initialization
    public void OnRestartButton () {
		SceneManager.LoadScene(0);
	}

    public void OnQuitButton()
    {
        Application.Quit();
    }

    void Start()
    {
       points = PlayerPrefs.GetInt("points");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("escape"))
        {
			Application.Quit();
        }
        
            GameInfo.GetComponent<GUIText>().text = "Zdobyłeś: " + points;
        
    }
}
