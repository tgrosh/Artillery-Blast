using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explodable: NetworkBehaviour {
    public ParticleSystem explosionPrefab;
    public float explosionForce;
    public float explosionRadius;
        
    public virtual void Explode()
    {        
        GameObject explosion = Instantiate(explosionPrefab.gameObject, transform.position, Quaternion.identity);
        explosion.GetComponent<Explosion>().Explode(explosionForce, explosionRadius);

        Destroy(explosion, explosionPrefab.main.duration);
    }
}
