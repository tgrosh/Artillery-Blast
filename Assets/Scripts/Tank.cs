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

    CannonBall incoming;
    float origCamFieldOfView;

    public override void OnStartClient()
    {
        foreach (Transform childTransform in transform)
        {
            if (childTransform.CompareTag("TankColor"))
            {
                childTransform.gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", playerColor);
            }
        }
        
        origCamFieldOfView = Camera.main.fieldOfView;

        base.OnStartClient();
    }

    public override void OnStartLocalPlayer()
    {
        Tank.localPlayer = this;
        
        cannon.angleSlider = GameObject.Find("AngleSlider").GetComponent<Slider>();
        cannon.powerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();

        GetComponent<SphereCollider>().enabled = false;

        base.OnStartLocalPlayer();
    }

    void FixedUpdate()
    {
        if (isLocalPlayer) return;

        if (incoming != null)
        {
            Camera.main.transform.root.GetComponent<TankCam>().lookAtTarget = incoming.transform;

            Camera.main.fieldOfView = Mathf.SmoothStep(Camera.main.fieldOfView, zoomInDistance, zoomSpeed);

            Time.timeScale = slowMoTimeScale;
            Time.fixedDeltaTime = deltaTimeRatio * Time.timeScale;            
        } else
        {
            Camera.main.transform.root.GetComponent<TankCam>().lookAtTarget = null;
            Camera.main.fieldOfView = origCamFieldOfView;
            Time.timeScale = 1f;
        }
    }

    // Update is called once per frame
    void Update () {
        
    }

    void OnTriggerEnter(Collider col)
    {
        CannonBall ball = col.GetComponentInParent<CannonBall>();

        if (ball != null)
        {
            incoming = ball;
        }
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
        if (!cannon.reloading)
        {
            cannon.Fire(power);
            Rpc_Fire();
        }
    }

    [ClientRpc]
    public void Rpc_Fire()
    {
        cannon.Rpc_Fire();
    }

    [ClientRpc]
    public void Rpc_Explode()
    {
        base.Explode();
        
        Camera.main.transform.root.GetComponent<TankCam>().lookAtTarget = null;
        Camera.main.fieldOfView = origCamFieldOfView;
        Time.timeScale = 1f;

        if (isLocalPlayer)
        {
            GameObject.FindObjectOfType<UI>().YouLose();
        } else
        {
            GameObject.FindObjectOfType<UI>().YouWin();
        }
    }

    [Server]
    public override void Explode()
    {
        Rpc_Explode();
    }
    
}
