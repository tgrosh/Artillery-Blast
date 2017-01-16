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
    public float focusDelay = .5f;
    public TankCam cam;

    [SyncVar(hook="SpawnPositionHook")]
    Vector3 spawnPosition;
    [SyncVar(hook="SpawnRotationHook")]
    Quaternion spawnRotation;

    bool exploded;
    CannonBall incoming;
    bool focusing;
    float currentFocusDelay = 0f;

    void Start()
    {
        if (isClient)
        {
            foreach (Transform childTransform in transform)
            {
                if (childTransform.CompareTag("TankColor"))
                {
                    childTransform.gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", playerColor);
                }
            }
        }

        if (isLocalPlayer)
        {
            Tank.localPlayer = this;

            cannon.angleSlider = GameObject.Find("AngleSlider").GetComponentInChildren<RadialSlider>();
            cannon.powerSlider = GameObject.Find("PowerSlider").GetComponentInChildren<VerticalSlider>();

            GetComponent<SphereCollider>().enabled = false;
        }
    }
        
    void FixedUpdate()
    {
        if (isLocalPlayer || cam == null) return;
        
        if (focusing)
        {
            currentFocusDelay = 0;
            cam.FocusOn(transform, 10, .25f);        
        } else if (currentFocusDelay < focusDelay)
        {
            currentFocusDelay += Time.deltaTime;
        } else
        {
            cam.ClearFocus();
        }
        
        focusing = (incoming != null);
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
    }

    [ClientRpc]
    public void Rpc_Explode()
    {
        exploded = true;
        base.Explode();
        cannon.angleSlider = null;

        EndGame();
    }

    void EndGame()
    {
        focusing = false;

        if (isLocalPlayer)
        {
            GameObject.FindObjectOfType<UI>().YouLose();
        }
        else
        {
            GameObject.FindObjectOfType<UI>().YouWin();
        }
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
