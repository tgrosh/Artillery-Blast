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

    [Server]
    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab.gameObject, transform.position, Quaternion.identity);
        NetworkServer.Spawn(explosion);
        explosion.GetComponent<Explosion>().Rpc_Explode();

        Destroy(gameObject, explosionPrefab.main.duration);
        Destroy(explosion, explosionPrefab.main.duration);
    }    
}
