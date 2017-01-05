using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetManager : NetworkManager {

    // called when a new player is added for a client
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Transform startPosition = GetStartPosition();
        GameObject player = Instantiate(playerPrefab, startPosition.position, startPosition.rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
