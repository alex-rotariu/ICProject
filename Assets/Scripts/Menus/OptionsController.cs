using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.8f;
    private int currentTrack;
    private MusicPlayer musicPlayer;

    // Start is called before the first frame update
    void Start() {
        volumeSlider.value = PlayerPrefsController.GetMasterVolume();
        currentTrack = PlayerPrefsController.GetMasterTrack();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        musicPlayer.PlaySong(currentTrack);
    }

    // Update is called once per frame
    void Update() {    
        if (musicPlayer) {
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
