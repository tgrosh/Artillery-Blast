using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public Wind windPrefab;

    [Server]
    void Start () {
        Wind wind = Instantiate(windPrefab, transform);
        NetworkServer.Spawn(wind.gameObject);
    }
}
