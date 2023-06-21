using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	public void OnPlayButton () {
		SceneManager.LoadScene(1);
	}

    public void OnOptionsButton()
    {
        
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

}
