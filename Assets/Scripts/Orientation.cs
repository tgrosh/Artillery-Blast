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
        Tank.OnTankReady += Tank_OnTankReady;
    }

    private void Tank_OnTankReady(Tank tank)
    {
        TankSetup();
    }

    // Use this for initialization
    void Start()
    {
        TankSetup();
    }

    private static void TankSetup()
    {
        if (Tank.localPlayer != null)
        {
            Tank.localPlayer.Cmd_SetPosition();
            foreach (Tank t in FindObjectsOfType<Tank>())
            {
                t.cam = Camera.main.transform.root.GetComponentInChildren<TankCam>();
            }
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

