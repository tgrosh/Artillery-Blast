using UnityEngine;
using System.Collections;

public class EnemyTank : Tank {

	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        GameObject.FindObjectOfType<UI>().YouWin();
    }
}
