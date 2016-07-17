using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {
    public float explosionRadius;
    public Detonator explosionPrefab;
    public Detonator explosionGroundPrefab;
    public AudioClip explosionAudioClip;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Contains("Tank"))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider collider in colliders)
            {
                Rigidbody body = collider.GetComponent<Rigidbody>();

                if (body != null)
                {
                    if (collider.gameObject.name != "Ground")
                    {
                        body.isKinematic = false;
                    }
                }
            }
            
            Debug.Log("Collision with " + col.gameObject.name);
            Explode(explosionPrefab, 1f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision with " + col.gameObject.name);
        Explode(explosionGroundPrefab, .5f);
    }

    void Explode(Detonator detonatorPrefab, float volume)
    {
        AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position, volume);
        Instantiate(detonatorPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
