using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownButton : MonoBehaviour {
    public Image progressFill;
    
    Button button;
    float cooldownTime;
    public bool isOnCooldown;
    float currentCooldownTime;
    ColorBlock origButtonColors;

    public float CurrentCooldownTime
    {
        get
        {
            return currentCooldownTime;
        }

        set
        {
            currentCooldownTime = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
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
    void Update()
    {
        if (isOnCooldown && cooldownTime > 0)
        {
            if (currentCooldownTime >= cooldownTime)
            {
                progressFill.gameObject.SetActive(false);
                button.interactable = true;
                currentCooldownTime = 0;
                isOnCooldown = false;
            }
            else
            {
                currentCooldownTime += Time.deltaTime;
                progressFill.fillAmount = currentCooldownTime / cooldownTime;
                progressFill.color = Color.Lerp(origButtonColors.disabledColor, origButtonColors.normalColor, progressFill.fillAmount);
            }
        }
    }
}
