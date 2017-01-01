using System;
using UnityEngine;
using UnityEngine.Networking;

public class CannonBall : Explodable
{
    [Server]
    void OnTriggerEnter(Collider col)
    {
        if (isServer)
        {
            Debug.Log("CannonBall.OnTriggerEnter");
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
    new void Explode()
    {
        Rpc_Explode();   
    }
}
