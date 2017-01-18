using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireButton : MonoBehaviour {
    public Button button;
    public Image buttonImage;
    public Slider progressSlider;
    public Image progressFill;
    public float reloadTime;

    bool reloading;
    float currentReloadTime;
    Color progressOrigColor;
    Color buttonColor;

    // Use this for initialization
    void Start () {
        button.onClick.AddListener(() => buttonOnClick());
        progressOrigColor = progressFill.color;
        buttonColor = buttonImage.color;
    }

    private void buttonOnClick()
    {
        if (reloadTime > 0)
        {
            progressFill.color = buttonColor;
            button.interactable = false;
            reloading = true;
        }
    }
    
    // Update is called once per frame
    void Update () {
		if (reloading && reloadTime > 0)
        {
            if (currentReloadTime > reloadTime)
            {
                button.interactable = true;
                currentReloadTime = 0;
                reloading = false;
                progressSlider.enabled = false;                
            } else
            {
                progressSlider.enabled = true;
                currentReloadTime += Time.deltaTime;
                progressSlider.value = currentReloadTime / reloadTime;
                progressFill.color = Color.Lerp(buttonColor, progressOrigColor, progressSlider.value);
            }
        }
	}
}
