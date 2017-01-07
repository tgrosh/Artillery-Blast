using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class CannonBall : Explodable
{
    void Start()
    {
        
    }

    [Server]
    void OnTriggerEnter(Collider col)
    {
        if (isServer && col.GetComponent<Tank>() == null)
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
    }

    [Server]
    public override void Explode()
    {
        Rpc_Explode();   
    }
}
