using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Tank : Explodable {
    [SyncVar]
    public Color playerColor;
    [SyncVar]
    public string playerName;
    public Cannon cannon;
    public static Tank localPlayer;
    public float zoomInDistance = 10f;
    public float zoomSpeed = .25f;
    public float slowMoTimeScale = .25f;
    public float deltaTimeRatio = .02f;
    public float clearFocusDelay = 2f;
    public TankCam cam;

    public static event TankReadyEventHandler OnTankReady;
    public delegate void TankReadyEventHandler(Tank tank);    

    [SyncVar(hook="SpawnPositionHook")]
    Vector3 spawnPosition;
    [SyncVar(hook="SpawnRotationHook")]
    Quaternion spawnRotation;
    Animator tankAnimator;

    bool exploded;

    void Start()
    {
        foreach (Transform childTransform in transform.FindChild("TankBody").transform)
        {
            if (childTransform.CompareTag("TankColor"))
            {
                childTransform.gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", playerColor);
            }
        }

        if (isLocalPlayer)
        {
            Tank.localPlayer = this;

            cannon.angleSlider = GameObject.Find("AngleSlider").GetComponentInChildren<RadialSlider>();
            cannon.powerSlider = GameObject.Find("PowerSlider").GetComponentInChildren<VerticalSlider>();            
        }

        tankAnimator = GetComponent<Animator>();

        Tank.OnTankReady(this);
    }
        
    void FixedUpdate()
    {
    }

    [Server]
    public void Focus(CannonBall ball)
    {
        Rpc_Focus();
    }

    [ClientRpc]
    public void Rpc_Focus()
    {
        cam.FocusOn(transform, 10, slowMoTimeScale);
    }

    [Server]
    public void UnFocus()
    {
        StartCoroutine(DelayedUnFocus());        
    }

    IEnumerator DelayedUnFocus()
    {
        Rpc_ResetFocus();
        yield return new WaitForSecondsRealtime(clearFocusDelay);
        Rpc_UnFocus();
    }

    [ClientRpc]
    public void Rpc_ResetFocus()
    {
        cam.ResetFocus();
    }

    [ClientRpc]
    public void Rpc_UnFocus()
    {
        cam.ClearFocus();
    }

    [Command]
    public void Cmd_SetPlayerInfo(string name, Color color)
    {
        this.playerName = name;
        this.playerColor = color;
    }
    
    [Command] //server
    public void Cmd_Fire(float power)
    {
        if (!cannon.reloading && !exploded)
        {
            cannon.Fire(power);
            Rpc_Fire();
        }
    }

    [ClientRpc]
    public void Rpc_Fire()
    {
        cannon.ShowCannonFire();
        tankAnimator.SetTrigger("Fire");
    }

    [ClientRpc]
    public void Rpc_Explode()
    {
        tankAnimator.StopPlayback();
        exploded = true;
        base.Explode();
        cannon.angleSlider = null;
        cam.gameObject.GetComponent<CameraShaker>().Shake();

        StartCoroutine(EndGame());
    }

    [Client]
    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(3f);
        if (isLocalPlayer)
        {
            GameObject.FindObjectOfType<UI>().YouLose();
        }
        else
        {
            GameObject.FindObjectOfType<UI>().YouWin();
        }
    }
    
    [Command]
    public void Cmd_ReturnToLobby()
    {
        GameObject.FindObjectOfType<LobbyMan>().ServerReturnToLobby();
    }

    [Server]
    public override void Explode()
    {
        exploded = true;
        Rpc_Explode();
    }
    
    [Command]
    public void Cmd_SetPosition()
    {
        //set tank position variables
        Transform pos = LobbyMan.singleton.GetStartPosition();
        spawnPosition = pos.position;
        spawnRotation = pos.rotation;
    }

    void SpawnPositionHook(Vector3 position)
    {
        //hook from syncvar
        transform.position = position;
    }

    void SpawnRotationHook(Quaternion rotation)
    {
        //hook from syncvar
        transform.rotation = rotation;
    }
}
