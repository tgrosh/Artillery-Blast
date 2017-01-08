using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RadialSlider: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public int minValue;
    public int maxValue;
    public int increment;

    bool isPointerDown = false;
    RectTransform rect;
    Text text;
    Vector2 localPos; // Mouse position 

    void Start()
    {
        value = minValue;
        text = GetComponentInChildren<Text>();
        rect = transform as RectTransform;

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
		//Debug.Log("mousedown");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPointerDown= false;
		//Debug.Log("mousedown");
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

                    // local pos is the mouse position.
                    float angle = (Mathf.Atan2(-localPos.y, localPos.x) * 180f / Mathf.PI + 180f) / 360f; //number between 0 and 1

                    if (minValue != maxValue)
                    {
                        angle = Mathf.Clamp(angle * 360f, minValue, maxValue) / 360f;
                    }

                    value = ((int)(angle * 360f));

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
        GetComponent<Image>().fillAmount = value / 360f;
        GetComponent<Image>().color = Color.Lerp(Color.green, Color.yellow, (value / 360f) / (maxValue/360f));
        text.text = value.ToString();
    }
}
