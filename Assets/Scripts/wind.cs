using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class wind : MonoBehaviour {
    public Vector3 magnitude;
    public float windScale;
    public float mediumSpeedThreshold;
    public float highSpeedThreshold;
    public Color lowSpeedColor;
    public Color mediumSpeedColor;
    public Color highSpeedColor;

    List<CannonBall> cannonBallsAffected = new List<CannonBall>();
    Image windSpeedImage;
    Image windSpeedIndicatorImage;
    Text windSpeedText;

	// Use this for initialization
	void Start () {
        windSpeedImage = GameObject.Find("WindSpeedImage").GetComponent<Image>();
        windSpeedIndicatorImage = GameObject.Find("WindSpeedIndicator").GetComponent<Image>();
        windSpeedText = GameObject.Find("WindSpeed").GetComponent<Text>();

        if (magnitude.x < 0)
        {
            windSpeedImage.transform.Rotate(Vector3.up, 180);
        }

        if (Math.Abs(magnitude.x) > highSpeedThreshold)
        {
            windSpeedImage.color = highSpeedColor;
            windSpeedIndicatorImage.color = highSpeedColor;                        
        } else if (Math.Abs(magnitude.x) > mediumSpeedThreshold)
        {
            windSpeedImage.color = mediumSpeedColor;
            windSpeedIndicatorImage.color = mediumSpeedColor;
        } else
        {
            windSpeedImage.color = lowSpeedColor;
            windSpeedIndicatorImage.color = lowSpeedColor;
        }
        windSpeedText.color = new Color(windSpeedImage.color.r / 3, windSpeedImage.color.g / 3, windSpeedImage.color.b / 3);
        windSpeedText.text = magnitude.x + " mph";
    }
	
	// Update is called once per frame
	void Update () {
	    foreach (CannonBall cannonBall in GameObject.FindObjectsOfType<CannonBall>())
        {
            if (!cannonBallsAffected.Contains(cannonBall))
            {
                cannonBallsAffected.Add(cannonBall);
                cannonBall.gameObject.GetComponent<Rigidbody>().AddForce(magnitude * windScale);
            }            
        }
	}
}
