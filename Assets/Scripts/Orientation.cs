using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Orientation : NetworkBehaviour {
    public GameObject cameraDolly;
    public SpawnPosition leftSpawn;
    public SpawnPosition rightSpawn;

    void Awake()
    {
        //cameraDolly.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        Tank.localPlayer.Cmd_SetPosition();
        foreach (Tank t in FindObjectsOfType<Tank>())
        {
            t.cam = Camera.main.transform.root.GetComponentInChildren<TankCam>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}    
}
