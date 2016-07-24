using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class wind : MonoBehaviour {
    public int minSpeed;
    public int maxSpeed;
    public float windScale;
    public float mediumSpeedThreshold;
    public float highSpeedThreshold;
    public Color lowSpeedColor;
    public Color mediumSpeedColor;
    public Color highSpeedColor;
    public AudioSource windLowAudio;
    public AudioSource windMediumAudio;
    public AudioSource windHighAudio;

    int magnitude;
    List<CannonBall> cannonBallsAffected = new List<CannonBall>();
    Image windSpeedImage;
    Image windSpeedIndicatorImage;
    Text windSpeedText;

	// Use this for initialization
	void Start () {
        magnitude = UnityEngine.Random.Range(minSpeed, maxSpeed);

        windSpeedImage = GameObject.Find("WindSpeedImage").GetComponent<Image>();
        windSpeedIndicatorImage = GameObject.Find("WindSpeedIndicator").GetComponent<Image>();
        windSpeedText = GameObject.Find("WindSpeed").GetComponent<Text>();

        if (magnitude < 0)
        {
            windSpeedImage.transform.Rotate(Vector3.up, 180);
        }

        if (Math.Abs(magnitude) > highSpeedThreshold)
        {
            windSpeedImage.color = highSpeedColor;
            windSpeedIndicatorImage.color = highSpeedColor;
            windHighAudio.Play();                      
        } else if (Math.Abs(magnitude) > mediumSpeedThreshold)
        {
            windSpeedImage.color = mediumSpeedColor;
            windSpeedIndicatorImage.color = mediumSpeedColor;
            windMediumAudio.Play();
        } else
        {
            windSpeedImage.color = lowSpeedColor;
            windSpeedIndicatorImage.color = lowSpeedColor;
            windLowAudio.Play();
        }
        windSpeedText.color = new Color(windSpeedImage.color.r / 3, windSpeedImage.color.g / 3, windSpeedImage.color.b / 3);
        windSpeedText.text = magnitude + " mph";
    }
	
	// Update is called once per frame
	void Update () {
	    foreach (CannonBall cannonBall in GameObject.FindObjectsOfType<CannonBall>())
        {
            if (!cannonBallsAffected.Contains(cannonBall))
            {
                cannonBallsAffected.Add(cannonBall);
                cannonBall.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(magnitude, 0, 0) * windScale);
            }            
        }
	}
}
