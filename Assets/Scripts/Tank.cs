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
    
    public override void OnStartLocalPlayer()
    {
        Tank.localPlayer = this;

        //this.playerName = MenuPlayer.current.playerName;
        //this.playerColor = MenuPlayer.current.playerColor;

        //this.Cmd_SetPlayerInfo(this.playerName, this.playerColor);

        cannon.angleSlider = GameObject.Find("AngleSlider").GetComponent<Slider>();
        cannon.powerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();

        base.OnStartLocalPlayer();
    }
    
    // Update is called once per frame
    void Update () {
        foreach (Transform childTransform in transform)
        {
            if (childTransform.CompareTag("TankColor"))
            {
                childTransform.gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", playerColor);
            }
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
