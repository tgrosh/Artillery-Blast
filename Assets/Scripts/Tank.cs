using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {
    public Color color;

	// Use this for initialization
	void Start () {
	    foreach (Transform childTransform in transform)
        {
            if (childTransform.CompareTag("PlayerColor"))
            {
                childTransform.gameObject.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", color);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
