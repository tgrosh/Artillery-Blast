using UnityEngine;
using UnityEngine.Networking;

public class CannonBall : Explodable
{
    [Server]
    void OnTriggerEnter(Collider col)
    {
        Explode(col.gameObject);
    }
    
    [Server]
    void OnCollisionEnter(Collision col)
    {
        Explode(col.gameObject);
    }
    
    [Server]
    void Explode(GameObject collider)
    {
        Explodable explodable = collider.GetComponentInParent<Explodable>();

        if (explodable != null)
        {
            explodable.Explode();
        }

        base.Explode();
        Destroy(gameObject);
    }
}
