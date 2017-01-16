using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
    public Wind windPrefab;
    public Orientation[] orientations;
    
    void Awake()
    {
        if (GameObject.FindObjectOfType<NetworkManager>() == null)
        {
            SceneManager.LoadScene(0);
        }
    }
    
    void Start () {
        
    }
    
    public override void OnStartServer()
    {
        Wind wind = Instantiate(windPrefab, transform);
        NetworkServer.Spawn(wind.gameObject);

        base.OnStartServer();
    }      
}
