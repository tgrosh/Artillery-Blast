using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Cannon : NetworkBehaviour {
    public GameObject projectilePrefab;
    public ParticleSystem cannonFirePrefab;
    public AudioSource cannonFireSound;
    public AudioSource cannonReloadSound;
    public Transform projectileSpawner;
    public float reloadTime;
    public Image reloadImage;
    public float powerScale = 1.0f;

    [HideInInspector]
    public ArtillerySlider angleSlider;
    [HideInInspector]
    public ArtillerySlider powerSlider;
    [HideInInspector]
    public bool reloading;

    float currentLoadTime = 0f;

    // Use this for initialization
    void Start () {
        reloadImage.transform.parent.parent.gameObject.SetActive(false);
    }
	
	void Update () {        
        if (angleSlider != null)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(-angleSlider.value, 0, 0));
        }

        if (reloading)
        {
            reloadImage.transform.parent.parent.gameObject.SetActive(true);
            currentLoadTime += Time.deltaTime;
            reloadImage.fillAmount = currentLoadTime / reloadTime;

            if (currentLoadTime >= reloadTime)
            {
                cannonReloadSound.Stop();
                reloading = false;
                currentLoadTime = 0f;
                reloadImage.transform.parent.parent.gameObject.SetActive(false);
            }
        }
	}
    
    [Server]
    public void Fire(float power)
    {
        if (!reloading)
        {            
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, projectileSpawner.position, gameObject.transform.localRotation);
            projectile.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * power * powerScale, ForceMode.Impulse);
            NetworkServer.Spawn(projectile);

            reloading = true;
        }
    }
    
    public void ShowCannonFire()
    {
        cannonFireSound.Play(); //needs to happen on client
        ParticleSystem cannonFire = Instantiate(cannonFirePrefab, projectileSpawner.position, gameObject.transform.localRotation);
        Destroy(cannonFire.gameObject, cannonFire.main.duration);
        cannonReloadSound.Play(); //needs to happen on client
    }
}
