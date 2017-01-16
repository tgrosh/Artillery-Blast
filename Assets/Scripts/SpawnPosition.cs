using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPosition : MonoBehaviour {
    public Text playerText;
    public Tank player;

	// Use this for initialization
	void Start () {
        transform.Find("SpawnAvatar").gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerEnter(Collider col)
    {
        Tank tank = col.GetComponentInParent<Tank>();
        if (tank != null)
        {
            player = tank;
            playerText.text = player.playerName;
        }
    }
}
