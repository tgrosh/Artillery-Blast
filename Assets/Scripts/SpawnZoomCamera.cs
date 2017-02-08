using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoomCamera : MonoBehaviour {
    public GameObject spawnPointCanvas;
    public GameObject cameraDolly;

    GameObject uiPanel;

    void Awake()
    {
        uiPanel = GameObject.Find("UIPanel");
    }

    // Update is called once per frame
    void Update () {
		if ((Input.GetKey(KeyCode.LeftArrow) && transform.parent.name == "SpawnLeft") || 
                (Input.GetKey(KeyCode.RightArrow) && transform.parent.name == "SpawnRight"))
        {
            spawnPointCanvas.SetActive(false);
            uiPanel.SetActive(false);
            cameraDolly.SetActive(false);
            GetComponent<Camera>().enabled = true;          
        } else if ((!Input.GetKey(KeyCode.RightArrow) && transform.parent.name == "SpawnLeft") ||
                (!Input.GetKey(KeyCode.LeftArrow) && transform.parent.name == "SpawnRight"))
        {
            GetComponent<Camera>().enabled = false;
            spawnPointCanvas.SetActive(true);
            uiPanel.SetActive(true);
            cameraDolly.SetActive(true);
        }
    }
}
