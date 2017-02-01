using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
    public static GameManager instance;

    public Orientation[] orientations;
    public bool isGameOver;
    
    void Awake()
    {
        instance = this;

        if (GameObject.FindObjectOfType<NetworkManager>() == null)
        {
            SceneManager.LoadScene(0);
        }
    }
    
    void Start () {
        
    }       
}
