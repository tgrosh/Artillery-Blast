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
    public GameObject uiPanel;
    public GameObject endGameMenu;

    // Use this for initialization
    void Start () {      
    }

    public void ShowGameUI(Orientation orientation, float delay)
    {
        StartCoroutine(ShowGameUIDelayed(orientation, delay));
    }

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ReturnToLobby()
    {
        Tank.localPlayer.Cmd_ReturnToLobby();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void YouWin()
    {
        endGameMenu.GetComponent<Animator>().Play("EndGameWinner");
        uiPanel.GetComponent<Animator>().Play("UIPanelExit");
    }

    public void YouLose()
    {
        endGameMenu.GetComponent<Animator>().Play("EndGameDefeated");
        uiPanel.GetComponent<Animator>().Play("UIPanelExit");
    }

    public void Fire()
    {
        Tank.localPlayer.Cmd_Fire(Tank.localPlayer.cannon.powerSlider.value);
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

    IEnumerator ShowGameUIDelayed(Orientation orientation, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (uiPanel != null)
        {
            uiPanel.GetComponent<Animator>().enabled = true;
            StartCoroutine(FadeIn(orientation.leftSpawn.canvasGroup, 2f));
            StartCoroutine(FadeIn(orientation.rightSpawn.canvasGroup, 2f));
        }
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float fadeSpeed)
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, float fadeSpeed)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}
