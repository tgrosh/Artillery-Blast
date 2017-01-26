using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public GameObject youWinPrefab;
    public GameObject youLosePrefab;

    public AudioSource uiAudio;

    // Use this for initialization
    void Start () {
        uiAudio = transform.Find("UIAudioSource").GetComponent<AudioSource>();
	}

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void EndGame()
    {
        Tank.localPlayer.Cmd_ReturnToLobby();
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
        NetManager.singleton.StopHost();
    }

    public void Rematch()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
        
    public static void Log(string message)
    {
        GameObject DebugLogText = GameObject.Find("DebugLogText");

        if (DebugLogText != null)
        {
            DebugLogText.GetComponent<Text>().text += "\n" + message;
        }
        Debug.Log(message);
    }
}
