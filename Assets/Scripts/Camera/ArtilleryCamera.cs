using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Cameras;

public class ArtilleryCamera : MonoBehaviour {
    public FreeLookCam freeLook;

	// Use this for initialization
	void Start () {
        freeLook = GetComponent<FreeLookCam>();
    }

    void Update()
    {        
        if (Input.GetButton("Fire2"))
        {
            freeLook.enabled = true;
        } else {
            freeLook.enabled = false;
        }
    }
}
