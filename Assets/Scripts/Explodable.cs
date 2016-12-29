using UnityEngine;
using System.Collections;

public class Explodable : MonoBehaviour {
    //public Detonator detonatorPrefab;
    public AudioClip explosionAudioClip;
    [Range(0, 1)]
    public float explosionVolume;
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Explode()
    {
        Collider[] colliders = transform.GetComponentsInChildren<Collider>(); // Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody body = collider.GetComponent<Rigidbody>();

            if (body != null)
            {
                body.isKinematic = false;
            }
        }

        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
        //Instantiate(detonatorPrefab, transform.position, Quaternion.identity);
        //Destroy(gameObject, detonatorPrefab.destroyTime);
        Destroy(gameObject);
    }
}
