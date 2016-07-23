﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cannon : MonoBehaviour {
    public GameObject projectilePrefab;
    public Detonator cannonFirePrefab;
    public AudioClip cannonFireSound;
    public Transform projectileSpawner;
    public float reloadTime;

    Slider angleSlider;
    Slider powerSlider;
    float currentLoadTime = 0f;
    bool reloading; 

    // Use this for initialization
    void Start () {
        angleSlider = GameObject.Find("AngleSlider").GetComponent<Slider>();
        powerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = Quaternion.Euler(new Vector3(-angleSlider.value, 0, 0));

        if (reloading)
        {
            currentLoadTime += Time.deltaTime;

            if (currentLoadTime >= reloadTime)
            {
                reloading = false;
                currentLoadTime = 0f;
            }
        }
	}

    public void Fire()
    {
        if (!reloading)
        {
            AudioSource.PlayClipAtPoint(cannonFireSound, projectileSpawner.position);
            Instantiate(cannonFirePrefab, projectileSpawner.position, gameObject.transform.localRotation);
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, projectileSpawner.position, gameObject.transform.localRotation);
            projectile.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * powerSlider.value, ForceMode.Impulse);

            reloading = true;
        }
    }
}
