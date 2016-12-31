using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Tank : NetworkBehaviour {
    [SyncVar]
    public Color color;
    public Cannon cannon;
    public static Tank localPlayer;

	// Use this for initialization
	public void Start()
    {
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
        Tank.localPlayer = this;

        cannon.angleSlider = GameObject.Find("AngleSlider").GetComponent<Slider>();
        cannon.powerSlider = GameObject.Find("PowerSlider").GetComponent<Slider>();

        base.OnStartLocalPlayer();
    }
    
    // Update is called once per frame
    void Update () {
	
	}
    
    [Command] //server
    public void Cmd_Fire(float power)
    {
        cannon.Fire(power);
    }
}
