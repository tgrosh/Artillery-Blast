﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class Wind : NetworkBehaviour {
    public int minSpeed;
    public int maxSpeed;
    public float windScale;
    public float mediumSpeedThreshold;
    public float highSpeedThreshold;
    public AudioSource windLowAudio;
    public AudioSource windMediumAudio;
    public AudioSource windHighAudio;
    public WindAxis axis;

    [SyncVar]
    int magnitude;
    List<CannonBall> cannonBallsAffected = new List<CannonBall>();
        
    public override void OnStartServer()
    {        
        magnitude = UnityEngine.Random.Range(minSpeed, maxSpeed);

        if (UnityEngine.Random.value > .5f)
        {
            magnitude *= -1;
        }

        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        WindVane windVane = FindObjectOfType<WindVane>();

        if (Math.Abs(magnitude) > highSpeedThreshold)
        {
            windVane.Show(magnitude, WindSpeed.High);
            windHighAudio.Play();
        }
        else if (Math.Abs(magnitude) > mediumSpeedThreshold)
        {
            windVane.Show(magnitude, WindSpeed.Medium);
            windMediumAudio.Play();
        }
        else
        {
            windVane.Show(magnitude, WindSpeed.Low);
            windLowAudio.Play();
        }

        base.OnStartClient();
    }
    
	void Update () {
        if (!isServer)
        {
            return;
        }

        foreach (CannonBall cannonBall in GameObject.FindObjectsOfType<CannonBall>())
        {
            if (!cannonBallsAffected.Contains(cannonBall))
            {
                cannonBallsAffected.Add(cannonBall);
                Vector3 windDirection = Vector3.zero;

                if (axis == WindAxis.x)
                {
                    windDirection = Vector3.right;
                } else if (axis == WindAxis.z)
                {
                    windDirection = Vector3.forward;
                } else if (axis == WindAxis.xinverse)
                {
                    windDirection = Vector3.left;
                }
                else if (axis == WindAxis.zinverse)
                {
                    windDirection = Vector3.back;
                }

                cannonBall.gameObject.GetComponent<Rigidbody>().AddForce(windDirection * magnitude * windScale);
            }
        }
	}
}

public enum WindAxis
{
    x,
    z,
    xinverse,
    zinverse
}

public enum WindSpeed
{
    High, Medium, Low
}