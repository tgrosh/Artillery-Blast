using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class CannonBall : Explodable
{
    [SyncVar]
    public NetworkInstanceId owner;

    Transform target;
    float distanceToTarget;
    float origCamFieldOfView;
    float fullCamFieldOfView = 10f;
    float zoomSpeed = .025f;  
    float origDistanceToTarget;
    float distancePercent;
    float newFieldOfView;

    void Start()
    {
        origCamFieldOfView = Camera.main.fieldOfView;

        if (owner == Tank.localPlayer.netId)
        {
            foreach (Tank tank in GameObject.FindObjectsOfType<Tank>())
            {
                if (!tank.isLocalPlayer)
                {
                    target = tank.transform;
                    origDistanceToTarget = distanceToTarget = Vector3.Distance(target.position, transform.position);
                }
            }            
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);
            distancePercent = (origDistanceToTarget - distanceToTarget) / origDistanceToTarget;

            if (distancePercent > .8f)
            {
                Camera.main.transform.root.GetComponent<TankCam>().lookAtTarget = target.transform;
                
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fullCamFieldOfView, zoomSpeed);

                Time.timeScale = .25f;
            }
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
        if (owner == Tank.localPlayer.netId)
        {
            Camera.main.transform.root.GetComponent<TankCam>().lookAtTarget = null;
            Camera.main.fieldOfView = origCamFieldOfView;
            Time.timeScale = 1f;
        }

        base.Explode();
        Destroy(gameObject);
    }

    [Server]
    public override void Explode()
    {
        Rpc_Explode();   
    }
}
