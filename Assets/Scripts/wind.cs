﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Wind : NetworkBehaviour {
    public int minSpeed;
    public int maxSpeed;
    public float windScale;
    public float gustIntervalMin;
    public float gustIntervalMax;
    public float gustDurationMin;
    public float gustDurationMax;
    public float gustScaleMin;
    public float gustScaleMax;
    public float mediumSpeedThreshold;
    public float highSpeedThreshold;
    public AudioSource windLowAudio;
    public AudioSource windMediumAudio;
    public AudioSource windHighAudio;
    public WindAxis axis;

    [SyncVar]
    int magnitude = 0;
    int baseMagnitude = 0;
    List<CannonBall> cannonBallsAffected = new List<CannonBall>();

    Gust nextGust;

    float currentWindTime;
    float currentGustTime;
    bool isGusting;
    bool isWindVaneUpdated;
        
    void Start()
    {
        if (isServer)
        {
            magnitude = Random.Range(minSpeed, maxSpeed);
            nextGust = GetNextGust();
            
            if (UnityEngine.Random.value > .5f)
            {
                magnitude *= -1;
            }

            baseMagnitude = magnitude;
        }
    }

    [ClientRpc]
    void Rpc_ShowWindVane(int magnitude)
    {
        WindVane windVane = FindObjectOfType<WindVane>();

        if (System.Math.Abs(magnitude) > highSpeedThreshold)
        {
            windVane.Show(magnitude, WindSpeed.High, axis);
            windMediumAudio.Stop();
            windLowAudio.Stop();
            windHighAudio.Play();
        }
        else if (System.Math.Abs(magnitude) > mediumSpeedThreshold)
        {
            windVane.Show(magnitude, WindSpeed.Medium, axis);
            windHighAudio.Stop();
            windLowAudio.Stop();
            windMediumAudio.Play();
        }
        else
        {
            windVane.Show(magnitude, WindSpeed.Low, axis);
            windHighAudio.Stop();
            windMediumAudio.Stop();
            windLowAudio.Play();
        }
    }
        
	void Update () {
        if (!isServer)
        {
            return;
        }
                
        if (isGusting)
        {
            currentGustTime += Time.deltaTime;
            
            if (currentGustTime > nextGust.duration)
            {
                //Debug.Log("Gust has ended after " + currentGustTime);
                isWindVaneUpdated = false;
                isGusting = false;
                currentGustTime = 0;
                magnitude = baseMagnitude;
                nextGust = GetNextGust();
                //Debug.Log("Next gust in " + nextGust.start + " seconds will be for " + nextGust.duration + " second and will scale to " + nextGust.scale);
            }
        } else
        {
            currentWindTime += Time.deltaTime;
            if (currentWindTime > nextGust.start)
            {
                //Debug.Log("Gust has started after " + currentWindTime);
                isWindVaneUpdated = false;
                isGusting = true;
                currentWindTime = 0;
                magnitude = Mathf.RoundToInt(baseMagnitude * nextGust.scale);
            }
        }

        if (!isWindVaneUpdated)
        {
            Rpc_ShowWindVane(magnitude);
            isWindVaneUpdated = true;
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

    Gust GetNextGust()
    {
        Gust result = new Gust();

        result.start = Random.Range(gustIntervalMin, gustIntervalMax);
        result.scale = Random.Range(gustScaleMin, gustScaleMax);
        result.duration = Random.Range(gustDurationMin, gustDurationMax);

        return result;
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

public class Gust
{
    public float start;
    public float scale;
    public float duration;
}