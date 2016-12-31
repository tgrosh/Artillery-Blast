using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explosion : NetworkBehaviour {
    public AudioClip explosionAudioClip;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    [ClientRpc]
    public void Rpc_Explode()
    {
        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);

        Collider[] colliders = transform.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            Rigidbody body = collider.GetComponent<Rigidbody>();

            if (body != null)
            {
                body.isKinematic = false;
            }
        }
    }
}
