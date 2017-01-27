using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class CannonBall : Explodable
{
    public float proximitySensorDelay;
    public AudioClip proximitySound;

    float currentProximitySensorDelay;
    bool proximitySensorActive;
    Tank proximityTarget;

    void Start()
    {
    }

    void Update()
    {
        if (currentProximitySensorDelay < proximitySensorDelay)
        {
            currentProximitySensorDelay += Time.deltaTime;
        } else if (!proximitySensorActive)
        {
            proximitySensorActive = true;
        }
    }

    [Server]
    void OnTriggerEnter(Collider col)
    {
        if (proximitySensorActive && proximityTarget == null)
        {
            proximityTarget = col.transform.root.GetComponent<Tank>();

            if (proximityTarget != null)
            {
                Rpc_ProximityAlert();
                proximityTarget.Focus(this);
            }
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

        if (proximityTarget != null)
        {
            proximityTarget.UnFocus();
        }
        Explode();
    }

    [ClientRpc]
    public void Rpc_ProximityAlert()
    {
        AudioSource.PlayClipAtPoint(proximitySound, Camera.main.transform.position);
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
