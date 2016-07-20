﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cannon : MonoBehaviour {
    public GameObject projectilePrefab;
    public Detonator cannonFirePrefab;
    public AudioClip cannonFireSound;

    Slider angleSlider;
    Slider powerSlider;
    Transform spawner;

    // Use this for initialization
    void Start () {
        angleSlider = GameObject.Find("AngleSlider").GetComponent<Slider>();
        powerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();
        spawner = transform.FindChild("ProjectileSpawner");
    }
	
	// Update is called once per frame
	void Update () {        
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angleSlider.value));
	}

    public void Fire()
    {
        AudioSource.PlayClipAtPoint(cannonFireSound, spawner.position);
        Instantiate(cannonFirePrefab, spawner.position, gameObject.transform.localRotation);
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, spawner.position, gameObject.transform.localRotation);
        projectile.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * powerSlider.value, ForceMode.Impulse);
    }
}
