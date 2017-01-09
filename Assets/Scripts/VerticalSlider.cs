using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VerticalSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public int minValue;
    public int maxValue;
    public int increment;

    Image[] bars;
    bool isPointerDown = false;
    RectTransform rect;
    Text text;
    int barsToShow;
    Vector2 localPos; // Mouse position 

    void Start()
    {
        value = minValue;
        text = GetComponentInChildren<Text>();
        rect = transform as RectTransform;
        bars = transform.Find("Foreground").GetComponentsInChildren<Image>();

        ShowValue();
    }
    
    // Called when the pointer enters our GUI component.
    // Start tracking the mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine("TrackPointer");
    }

    // Called when the pointer exits our GUI component.
    // Stop tracking the mouse
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("TrackPointer");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        //Debug.Log("mousedown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        //Debug.Log("mousedown");
    }

    IEnumerator TrackPointer()
    {
        var ray = GetComponentInParent<GraphicRaycaster>();
        var input = FindObjectOfType<StandaloneInputModule>();

        if (ray != null && input != null)
        {
            while (Application.isPlaying)
            {
                if (isPointerDown)
                {  
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, ray.eventCamera, out localPos);
                    
                    value = (int)(minValue + (((localPos.y / (rect.rect.height)) * maxValue) - minValue));
                    if (value < minValue)
                    {
                        value = minValue;
                    }
                    else if (value > maxValue)
                    {
                        value = maxValue;
                    }
                    if (increment > 0)
                    {
                        value = (int)(Mathf.Round(value / increment * 1f) * increment);
                    }

                    ShowValue();
                }

                yield return 0;
            }
        }
        else
            UnityEngine.Debug.LogWarning("Could not find GraphicRaycaster and/or StandaloneInputModule");
    }

    private void ShowValue()
    {
        text.text = value.ToString();

        barsToShow = (int)((1f * bars.Length / maxValue) * value);
        for (int x = 0; x < bars.Length; x++)
        {
            bars[x].color = Color.Lerp(Color.green, Color.yellow, (1f * value / maxValue));
            bars[x].enabled = (x > bars.Length - barsToShow - 1);
        }
    }
}
