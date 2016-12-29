using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cannon : MonoBehaviour {
    public GameObject projectilePrefab;
    //public Detonator cannonFirePrefab;
    public AudioSource cannonFireSound;
    public AudioSource cannonReloadSound;
    public Transform projectileSpawner;
    public float reloadTime;
    public Image reloadImage;
    public Slider angleSlider;
    public Slider powerSlider;

    float currentLoadTime = 0f;
    bool reloading; 

    // Use this for initialization
    void Start () {
        reloadImage.transform.parent.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (angleSlider != null)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(-angleSlider.value, 0, 0));
        }

        if (reloading)
        {
            reloadImage.transform.parent.gameObject.SetActive(true);
            currentLoadTime += Time.deltaTime;
            reloadImage.fillAmount = currentLoadTime / reloadTime;

            if (currentLoadTime >= reloadTime)
            {
                cannonReloadSound.Stop();
                reloading = false;
                currentLoadTime = 0f;
                reloadImage.transform.parent.gameObject.SetActive(false);
            }
        }
	}

    public void Fire()
    {
        if (!reloading)
        {
            cannonFireSound.Play();
            //Instantiate(cannonFirePrefab, projectileSpawner.position, gameObject.transform.localRotation);
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, projectileSpawner.position, gameObject.transform.localRotation);
            projectile.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * powerSlider.value, ForceMode.Impulse);

            reloading = true;
            cannonReloadSound.Play();
        }
    }
}
