using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArtillerySlider : MonoBehaviour {
    public Text text;
    public string suffix;

    private AudioSource uiAudioSource;
    private Slider slider;    

    // Use this for initialization
    void Start () {
        slider = GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        text.text = slider.value + suffix;
	}

    public void SliderValueChanged(float value)
    {
        
    }
}
