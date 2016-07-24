using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void LoadScene(int level)
    {
        //loadingImage.SetActive(true);
        SceneManager.LoadScene(level);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
