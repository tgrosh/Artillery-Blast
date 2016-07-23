using UnityEngine;

public class Billboard : MonoBehaviour { 
	void Update() { 
		transform.LookAt(Camera.main.transform.position);
        transform.Rotate(transform.up, 180);
	} 
}