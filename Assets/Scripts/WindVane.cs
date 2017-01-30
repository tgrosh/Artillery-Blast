using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindVane : MonoBehaviour
{
    public Color lowSpeedColor;
    public Color mediumSpeedColor;
    public Color highSpeedColor;
    
    Image windSpeedImage;
    Text windSpeedText;

    // Use this for initialization
    void Awake () {
        windSpeedImage = GameObject.Find("WindSpeedImage").GetComponent<Image>();
        windSpeedText = GameObject.Find("WindSpeed").GetComponent<Text>();        

    }
	
    public void Show(int magnitude, WindSpeed windSpeed, WindAxis axis)
    {
        if (magnitude < 0)
        {
            windSpeedImage.transform.rotation = Quaternion.identity;
        }

        if (windSpeed == WindSpeed.High)
        {
            windSpeedImage.color = windSpeedText.color = highSpeedColor;
        }
        else if (windSpeed == WindSpeed.Medium)
        {
            windSpeedImage.color = windSpeedText.color = mediumSpeedColor;
        }
        else
        {
            windSpeedImage.color = windSpeedText.color = lowSpeedColor;
        }
        
        windSpeedText.GetComponent<Outline>().effectColor = 
            windSpeedImage.GetComponent<Outline>().effectColor = 
            new Color(windSpeedImage.color.r / 3, windSpeedImage.color.g / 3, windSpeedImage.color.b / 3);

        windSpeedText.text = Math.Abs(magnitude).ToString();
    }
}
