using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireButton : MonoBehaviour {
    public Button button;
    public Image buttonImage;
    public Image progressFill;

    float cooldownTime;
    public bool isOnCooldown;
    float currentReloadTime;
    ColorBlock origButtonColors;

    // Use this for initialization
    void Start () {
        origButtonColors = button.colors;
    }

    public void Cooldown(float cooldownTime)
    {
        if (cooldownTime > 0)
        {
            progressFill.gameObject.SetActive(true);
            button.interactable = false;
            isOnCooldown = true;
            this.cooldownTime = cooldownTime;
        }
    }
        
    // Update is called once per frame
    void Update () {
		if (isOnCooldown && cooldownTime > 0)
        {
            if (currentReloadTime > cooldownTime)
            {
                progressFill.gameObject.SetActive(false);
                button.interactable = true;
                currentReloadTime = 0;
                isOnCooldown = false;              
            } else
            {
                currentReloadTime += Time.deltaTime;
                progressFill.fillAmount = currentReloadTime / cooldownTime;
                progressFill.color = Color.Lerp(origButtonColors.disabledColor, origButtonColors.normalColor, progressFill.fillAmount);
            }
        }
	}
    
}
