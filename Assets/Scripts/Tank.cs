using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Tank : NetworkBehaviour {
    public Color color;
    public Cannon cannon;

	// Use this for initialization
	public void Start() {
	    foreach (Transform childTransform in transform)
        {
            if (childTransform.CompareTag("TankColor"))
            {
                childTransform.gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", color);
            }
        }
	}

    public override void OnStartLocalPlayer()
    {
        cannon.angleSlider = GameObject.Find("AngleSlider").GetComponent<Slider>();
        cannon.powerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
