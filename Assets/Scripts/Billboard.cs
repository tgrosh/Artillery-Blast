using UnityEngine;

public class Billboard : MonoBehaviour { 
	void Update() {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(transform.up, 180);
        }
	} 
}