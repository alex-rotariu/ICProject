using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour {
    private AudioSource currentSource;
    private int sourcesNumber;
    private string[] trackNames = { "Loop Ambient008", "Loop Cinema002", "Loop Dance001", "Loop Dance004", "Loop Dreamy005", "Loop Electronic001", "Loop Pops001", "Loop Pops005", "Loop Rhythm003", "Loop Rock006"};
    private string songPath = "LoopAndMusicFree/Loop/";

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(this);
        sourcesNumber = trackNames.Length;
        AudioClip audioClip = Resources.Load(songPath + trackNames[0]) as AudioClip;
        currentSource = GetComponent<AudioSource>();
        currentSource.clip = audioClip;
        
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
        if(currentSource != null) {
            AudioClip audioClip = Resources.Load(songPath + trackNames[songIndex]) as AudioClip;
            currentSource.clip = audioClip;
            currentSource.Play();
        }
            

        return finish;
    }
}