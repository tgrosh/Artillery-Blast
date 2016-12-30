using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLobbyHook : LobbyHook {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Tank tank = gamePlayer.GetComponent<Tank>();
        tank.color = lobbyPlayer.GetComponent<LobbyPlayer>().playerColor;

        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
    }
}
