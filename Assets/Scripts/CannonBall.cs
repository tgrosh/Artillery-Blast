using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class CannonBall : Explodable
{
    [SyncVar]
    public NetworkInstanceId owner;

    void Start()
    {
        if (owner == Tank.localPlayer.netId)
        {
            Camera.main.transform.root.GetComponent<TankCam>().lookAtTarget = transform;
        }
    }
    

    [Server]
    void OnTriggerEnter(Collider col)
    {
        if (isServer)
        {
            Explode(col.gameObject);
        }
    }
    
    [Server]
    void OnCollisionEnter(Collision col)
    {
        if (isServer)
        {
            Explode(col.gameObject);
        }
    }
    
    [Server]
    void Explode(GameObject collider)
    {
        Explodable explodable = collider.GetComponentInParent<Explodable>();

        if (explodable != null)
        {
            explodable.Explode();
        }

        Explode();
    }

    [ClientRpc]
    public void Rpc_Explode()
    {
        base.Explode();
        Destroy(gameObject);
        if (owner == Tank.localPlayer.netId)
        {
            Camera.main.transform.root.GetComponent<TankCam>().lookAtTarget = null;
        }
    }

    [Server]
    public override void Explode()
    {
        Rpc_Explode();   
    }
}
