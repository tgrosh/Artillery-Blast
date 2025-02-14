﻿using UnityEngine;
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
    public GameObject waitingForPlayers;

    CooldownButton moveLeftButton;
    CooldownButton moveRightButton;
    CooldownButton fireButton;
    bool debugging;

    // Use this for initialization
    void Start () {
        fireButton = GameObject.Find("FireButton").GetComponent<CooldownButton>();
        moveLeftButton = GameObject.Find("MoveLeftButton").GetComponent<CooldownButton>();
        moveRightButton = GameObject.Find("MoveRightButton").GetComponent<CooldownButton>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyUp(KeyCode.F10))
        {
            debugging = !debugging;
        }

        if (debugging)
        {
            uiPanel.GetComponent<CanvasGroup>().alpha = 0f;
        } else
        {
            uiPanel.GetComponent<CanvasGroup>().alpha = 1f;
        }
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
        if (!fireButton.isOnCooldown)
        {
            Tank.localPlayer.Cmd_Fire(Tank.localPlayer.cannon.powerSlider.value);
            TriggerButtonCooldowns(Tank.localPlayer.cannon.reloadTime);
        }
    }

    public void MoveRight()
    {
        if (!moveRightButton.isOnCooldown)
        {
            Tank.localPlayer.Cmd_MoveRight();
            TriggerButtonCooldowns(Tank.localPlayer.movementCooldownTime);
        }
    }

    public void MoveLeft()
    {
        if (!moveLeftButton.isOnCooldown)
        {
            Tank.localPlayer.Cmd_MoveLeft();
            TriggerButtonCooldowns(Tank.localPlayer.movementCooldownTime);
        }
    }

    public void TriggerButtonCooldowns(float cooldownTime)
    {
        fireButton.Cooldown(cooldownTime);
        moveRightButton.Cooldown(cooldownTime);
        moveLeftButton.Cooldown(cooldownTime);
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

        Tank.localPlayer.Cmd_SetClientReady();

        yield return WaitForPlayers(orientation);
    }

    IEnumerator WaitForPlayers(Orientation orientation)
    {
        int readyTankCount = GetReadyTankCount();
        int neededTanks = 2;

        if (Application.isEditor)
        {
            neededTanks = 1;
        }

        while (readyTankCount < neededTanks)
        {
            waitingForPlayers.SetActive(true);            
            yield return null;
            readyTankCount = GetReadyTankCount();
        }

        //show GameUI
        if (uiPanel != null)
        {
            waitingForPlayers.SetActive(false);
            uiPanel.GetComponent<Animator>().enabled = true;
            StartCoroutine(FadeIn(orientation.leftSpawn.canvasGroup, 2f));
            StartCoroutine(FadeIn(orientation.rightSpawn.canvasGroup, 2f));
        }
    }

    int GetReadyTankCount()
    {
        Tank[] tanks = FindObjectsOfType<Tank>();

        int result = 0;
        foreach (Tank tank in tanks)
        {
            if (tank.isClientReady)
            {
                result++;
            }
        }

        return result;
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
