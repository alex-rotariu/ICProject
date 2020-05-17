using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
    Slider volumeSlider;
    float defaultVolume = 0.8f;
    private int currentTrack;
    private MusicPlayer musicPlayer;
    private bool isPlaying = false;
    private static OptionsController Instance;
    private float prevVolValue;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Instance.volumeSlider = FindObjectOfType<Slider>();
            if(Instance.volumeSlider)
                Instance.volumeSlider.value = PlayerPrefsController.GetMasterVolume();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        
        if (isPlaying == false)
        {
            volumeSlider = FindObjectOfType<Slider>();
            currentTrack = PlayerPrefsController.GetMasterTrack();
            musicPlayer = FindObjectOfType<MusicPlayer>();
            musicPlayer.PlaySong(currentTrack);
            prevVolValue = PlayerPrefsController.GetMasterVolume();
            musicPlayer.SetVolume(prevVolValue);
            isPlaying = true;
        }
        volumeSlider.value = prevVolValue;
    }
           

    // Update is called once per frame
    void Update() {
        
        if (musicPlayer && volumeSlider) {
            
            if(volumeSlider.value != prevVolValue)
            {
                musicPlayer.SetVolume(volumeSlider.value);
                prevVolValue = volumeSlider.value;
                PlayerPrefsController.SetMasterVolume(prevVolValue);
            }
                
        }
    }

    public void SkipSong() {
        PlayerPrefsController.SetMasterTrack(currentTrack);
        if (!musicPlayer.PlaySong(currentTrack+1))
            currentTrack++;
        else
            currentTrack = 0;
    }
}
