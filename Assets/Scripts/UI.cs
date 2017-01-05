using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Prototype.NetworkLobby;

public class UI : MonoBehaviour {
    public GameObject youWinPrefab;
    public GameObject youLosePrefab;

    // Use this for initialization
    void Start () {
	
	}

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void YouWin()
    {
        youWinPrefab.SetActive(true);
    }

    public void YouLose()
    {
        youLosePrefab.SetActive(true);
    }

    public void Fire()
    {
        Tank.localPlayer.Cmd_Fire(Tank.localPlayer.cannon.powerSlider.value);
    }

    public void NewMatch()
    {
        GameObject.FindObjectOfType<LobbyManager>().StopHost();
        GameObject.FindObjectOfType<LobbyManager>().StopClient();
        GameObject.FindObjectOfType<LobbyManager>().StopMatchMaker();
        LoadScene(2);
    }

    public void Rematch()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
