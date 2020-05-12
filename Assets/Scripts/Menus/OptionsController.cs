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

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        
        if (isPlaying == false)
        {
            volumeSlider = FindObjectOfType<Slider>();
            if(volumeSlider)
                
            currentTrack = PlayerPrefsController.GetMasterTrack();
            musicPlayer = FindObjectOfType<MusicPlayer>();
            musicPlayer.PlaySong(currentTrack);
            isPlaying = true;
        }
        volumeSlider.value = PlayerPrefsController.GetMasterVolume();
    }
           

    // Update is called once per frame
    void Update() {    
        if (musicPlayer && volumeSlider) {
            musicPlayer.SetVolume(volumeSlider.value);   
        }
    }

    public void SaveAndExit() {
        PlayerPrefsController.SetMasterVolume(volumeSlider.value);
        PlayerPrefsController.SetMasterTrack(currentTrack);
    }

    public void SkipSong() {
        if (!musicPlayer.PlaySong(currentTrack+1))
            currentTrack++;
        else
            currentTrack = 0;
    }

    public void SetDefaults() {
        volumeSlider.value = defaultVolume;
    }
}
