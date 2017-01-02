using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPosition : MonoBehaviour {
    public Text playerText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        Tank tank = col.GetComponentInParent<Tank>();
        if (tank != null)
        {
            playerText.text = tank.playerName;
        }
    }
}
