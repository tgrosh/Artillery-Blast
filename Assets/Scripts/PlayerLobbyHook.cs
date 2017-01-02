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
        tank.playerColor = lobbyPlayer.GetComponent<LobbyPlayer>().playerColor;
        tank.playerName = lobbyPlayer.GetComponent<LobbyPlayer>().playerName;

        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
    }
}
