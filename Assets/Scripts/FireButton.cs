using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireButton : MonoBehaviour {
    public Button button;
    public Image buttonImage;
    public Image progressFill;
    public float reloadTime;

    bool reloading;
    float currentReloadTime;
    ColorBlock origButtonColors;

    // Use this for initialization
    void Start () {
        origButtonColors = button.colors;
    }

    public void Cooldown()
    {
        if (reloadTime > 0)
        {
            progressFill.gameObject.SetActive(true);
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
                progressFill.gameObject.SetActive(false);
                button.interactable = true;
                currentReloadTime = 0;
                reloading = false;              
            } else
            {
                currentReloadTime += Time.deltaTime;
                progressFill.fillAmount = currentReloadTime / reloadTime;
                progressFill.color = Color.Lerp(origButtonColors.disabledColor, origButtonColors.normalColor, progressFill.fillAmount);
            }
        }
	}
}
