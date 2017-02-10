using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPosition : MonoBehaviour {
    public Text playerText;
    public Tank player;
    public CanvasGroup canvasGroup;
    public SpawnSide spawnSide;

	// Use this for initialization
	void Start () {
        transform.Find("SpawnAvatar").gameObject.SetActive(false);
        canvasGroup = transform.Find("Canvas").GetComponent<CanvasGroup>();
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
            playerText.color = player.playerColor;
            tank.spawnSide = spawnSide;
        }
    }
}

public enum SpawnSide
{
    Left,
    Right
}
