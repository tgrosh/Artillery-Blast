using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
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
}
