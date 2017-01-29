using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbyMan : LobbyManager {

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName == playScene)
        {
            Orientation[] orientations = FindObjectOfType<GameManager>().orientations;
            int orientationIndex = Random.Range(0, orientations.Length);

            Orientation selectedOrientation = orientations[orientationIndex];
            GameObject obj = Instantiate(selectedOrientation.gameObject);
            obj.SetActive(true);

            NetworkServer.Spawn(obj);

            base.OnServerSceneChanged(sceneName);
        }
    }
}
