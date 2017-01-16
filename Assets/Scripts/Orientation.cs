using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Orientation : NetworkBehaviour {
    public GameObject cameraDolly;
    public SpawnPosition leftSpawn;
    public SpawnPosition rightSpawn;
    public Wind windPrefab;
    public WindAxis windAxis;

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

    public override void OnStartServer()
    {
        Wind wind = Instantiate(windPrefab, transform);
        wind.axis = windAxis;
        NetworkServer.Spawn(wind.gameObject);

        base.OnStartServer();
    }

    // Update is called once per frame
    void Update () {
		
	}    
}

