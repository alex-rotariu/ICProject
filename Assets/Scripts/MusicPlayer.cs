using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour {
    private AudioSource[] audioSources;
    private AudioSource currentSource;
    private int sourcesNumber;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(this);
        audioSources = GetComponents<AudioSource>();
        sourcesNumber = audioSources.Length;
        currentSource = audioSources[0];
    }

    public void SetVolume(float volume) {
        if(currentSource)
            currentSource.volume = volume;
    }

    public bool PlaySong(int songIndex) {
        bool finish = false;
        if (songIndex == sourcesNumber) {
            songIndex = 0;
            finish = true;
        }
        if (currentSource) {
            currentSource.Stop();        
        }
        if(audioSources != null) {
            currentSource = audioSources[songIndex];
            currentSource.Play();
        }
            

        return finish;
    }
}