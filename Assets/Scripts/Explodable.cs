using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explodable: NetworkBehaviour {
    public Explosion explosionPrefab;
    public float explosionForce;
    public float explosionRadius;
        
    public virtual void Explode()
    {
        GameObject obj = Instantiate(explosionPrefab.gameObject, transform.position, Quaternion.identity);
        Explosion objExplosion = obj.GetComponent<Explosion>();

        objExplosion.Explode(explosionForce, explosionRadius);
        Destroy(obj, objExplosion.duration);
    }
}
