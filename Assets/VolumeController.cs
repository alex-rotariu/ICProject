using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private Text percentageText;
    private Slider volumeSlider;
    private float volumeSetting;

    void Start()
    {
        percentageText = transform.Find("Volume % Text").GetComponent<Text>();
        volumeSlider = transform.Find("Volume Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        volumeSetting = volumeSlider.value;
        percentageText.text = (Mathf.Ceil(volumeSetting * 100)).ToString() + "%"; 
    }
}
