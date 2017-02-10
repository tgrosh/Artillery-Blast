using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {
    bool debugging = false;
    GameObject cam;

	// Use this for initialization
	void Start () {
        cam = transform.Find("FirstPersonCamera").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.F9)) {
            debugging = !debugging;
        }
        cam.SetActive(debugging);
	}
}
