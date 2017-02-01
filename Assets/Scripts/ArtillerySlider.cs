using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ArtillerySlider: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public int minValue;
    public int maxValue;
    public int increment;
    public SliderType sliderType = SliderType.Radial;
    public AudioClip audioClip;

    AudioSource uiAudio;
    float recentValue;
    bool isPointerDown = false;
    RectTransform rect;
    Text text;
    Vector2 localPos; // Mouse position 

    void Start()
    {
        value = minValue;
        text = GetComponentInChildren<Text>();
        rect = transform as RectTransform;
        uiAudio = FindObjectOfType<UI>().uiAudio;

        ShowValue();
    }

    // Called when the pointer enters our GUI component.
    // Start tracking the mouse
    public void OnPointerEnter( PointerEventData eventData )
	{
		StartCoroutine( "TrackPointer" );            
	}
	
	// Called when the pointer exits our GUI component.
	// Stop tracking the mouse
	public void OnPointerExit( PointerEventData eventData )
	{
		StopCoroutine( "TrackPointer" );
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isPointerDown= true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPointerDown= false;
	}

	// mainloop
	IEnumerator TrackPointer()
	{
		var ray = GetComponentInParent<GraphicRaycaster>();
		var input = FindObjectOfType<StandaloneInputModule>();

		var text = GetComponentInChildren<Text>();

        if (ray != null && input != null)
        {
            while (Application.isPlaying)
            {
                if (isPointerDown)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, ray.eventCamera, out localPos);

                    if (sliderType == SliderType.Radial)
                    {
                        // local pos is the mouse position.
                        float angle = (Mathf.Atan2(-localPos.y, localPos.x) * 180f / Mathf.PI + 180f) / 360f; //number between 0 and 1

                        if (angle * 360f < maxValue && angle > 0)
                        {
                            if (minValue != maxValue)
                            {
                                angle = Mathf.Clamp(angle * 360f, minValue, maxValue) / 360f;
                            }

                            value = ((int)(angle * 360f));
                        }
                    } else
                    {
                        value = (int)(minValue + (((localPos.y / (rect.rect.height)) * maxValue) - minValue));
                        if (value < minValue)
                        {
                            value = minValue;
                        }
                        else if (value > maxValue)
                        {
                            value = maxValue;
                        }                        
                    }

                    if (increment > 0)
                    {
                        value = (int)(Mathf.Round(value / (increment * 1f)) * increment);
                    }

                    if (value != recentValue)
                    {
                        //value has changed
                        if (audioClip != null)
                        {
                            if (uiAudio.isPlaying)
                            {
                                uiAudio.Stop();
                            }
                            uiAudio.PlayOneShot(audioClip);
                        }
                    }
                    recentValue = value;

                    ShowValue();
                }

                yield return 0;
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("Could not find GraphicRaycaster and/or StandaloneInputModule");
        }
	}

    private void ShowValue()
    {
        if (sliderType == SliderType.Radial)
        {
            GetComponent<Image>().fillAmount = value / 360f;
            GetComponent<Image>().color = Color.Lerp(Color.green, Color.yellow, (value / 360f) / (maxValue / 360f));
        } else
        {
            GetComponent<Image>().fillAmount = value / 100f;
            GetComponent<Image>().color = Color.Lerp(Color.green, Color.yellow, (value / 100f) / (maxValue / 100f));
        }
        
        if (text != null)
        {
            text.text = value.ToString();
        }
    }
}

public enum SliderType
{
    Radial,
    Vertical
}
