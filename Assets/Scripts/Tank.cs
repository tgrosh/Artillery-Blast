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
    public GameObject tankFire;
    public SpawnSide spawnSide;
    [SyncVar]
    public bool isClientReady;
    public float movementCooldownTime;
    public float endGameZoomTime;
    [SyncVar]
    public int movementPosition = 0;

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

            cannon.angleSlider = GameObject.Find("AngleSlider").GetComponentInChildren<ArtillerySlider>();
            cannon.powerSlider = GameObject.Find("PowerSlider").GetComponentInChildren<ArtillerySlider>();            
        }

        tankAnimator = transform.Find("TankBody").GetComponent<Animator>();

        if (Tank.OnTankReady != null)
        {
            Tank.OnTankReady(this);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    void FixedUpdate()
    {
    }
        
    public void Zoom(float zoomTime)
    {
        cam.FocusOn(transform, zoomTime, 20, 1f);
    }
    
    public void Focus(float focusTime)
    {
        Rpc_Focus(focusTime);
    }
    
    public override void Explode()
    {
        exploded = true;
        Rpc_Explode();
    }
    
    public void UnFocus()
    {
        StartCoroutine(DelayedUnFocus());
    }
    
    [Command]
    public void Cmd_SetClientReady()
    {
        isClientReady = true;
    }

    [Command]
    public void Cmd_Fire(float power)
    {
        if (!cannon.reloading && !exploded)
        {
            cannon.Fire(power);
            Rpc_Fire();
        }
    }

    [Command]
    public void Cmd_ReturnToLobby()
    {
        GameObject.FindObjectOfType<LobbyMan>().ServerReturnToLobby();
    }

    [Command]
    public void Cmd_SetPosition()
    {
        if (transform.position == Vector3.zero)
        {
            //set tank position variables
            Transform pos = LobbyMan.singleton.GetStartPosition();
            spawnPosition = pos.position;
            spawnRotation = pos.rotation;
        }
    }

    [Command]
    public void Cmd_MoveRight()
    {
        if (movementPosition < 1)
        {
            movementPosition++;
            Rpc_MoveRight();
        }
    }

    [Command]
    public void Cmd_MoveLeft()
    {
        if (movementPosition > -1)
        {
            movementPosition--;
            Rpc_MoveLeft();
        }
    }

    [ClientRpc]
    public void Rpc_MoveRight()
    {
        if (spawnSide == SpawnSide.Right)
        {
            tankAnimator.SetTrigger("MoveBackward");
            StartCoroutine(DelayedAnimationPosition(1f, movementPosition * -1));
        } else
        {   
            tankAnimator.SetTrigger("MoveForward");
            StartCoroutine(DelayedAnimationPosition(1f, movementPosition));
        }
    }

    [ClientRpc]
    public void Rpc_MoveLeft()
    {
        if (spawnSide == SpawnSide.Right)
        {
            tankAnimator.SetTrigger("MoveForward");
            StartCoroutine(DelayedAnimationPosition(1f, movementPosition * -1));
        }
        else
        {
            tankAnimator.SetTrigger("MoveBackward");
            StartCoroutine(DelayedAnimationPosition(1f, movementPosition));
        }
    }

    [ClientRpc]
    public void Rpc_Focus(float focusTime)
    {
        cam.FocusOn(transform, focusTime, 10, slowMoTimeScale);
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
        Instantiate(tankFire, transform.position, Quaternion.identity);

        if (!GameManager.instance.isGameOver)
        {
            StartCoroutine(EndGame());
        }
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

    IEnumerator DelayedAnimationPosition(float delay, int position)
    {
        yield return new WaitForSecondsRealtime(delay);
        tankAnimator.SetInteger("position", position);
    }

    IEnumerator DelayedUnFocus()
    {
        Rpc_ResetFocus();
        yield return new WaitForSecondsRealtime(clearFocusDelay);
        Rpc_UnFocus();
    }
    
    IEnumerator EndGame()
    {
        GameManager.instance.isGameOver = true;

        yield return new WaitForSecondsRealtime(3f);

        Tank.localPlayer.Zoom(endGameZoomTime);
        if (isLocalPlayer)
        {
            GameObject.FindObjectOfType<UI>().YouLose();
        }
        else
        {
            GameObject.FindObjectOfType<UI>().YouWin();
        }
    }
}

