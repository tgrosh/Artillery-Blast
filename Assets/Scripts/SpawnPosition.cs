using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPosition : MonoBehaviour {
    public Text playerText;
    public Tank player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            playerText.text = player.playerName;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Tank tank = col.GetComponentInParent<Tank>();
        if (tank != null)
        {
            player = tank;
        }
    }
}
