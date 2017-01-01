using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explodable : NetworkBehaviour {
    public ParticleSystem explosionPrefab;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab.gameObject, transform.position, Quaternion.identity);
        explosion.GetComponent<Explosion>().Explode();

        Destroy(explosion, explosionPrefab.main.duration);
    }
}
