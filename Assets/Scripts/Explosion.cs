using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public AudioClip explosionAudioClip;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Explode()
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
