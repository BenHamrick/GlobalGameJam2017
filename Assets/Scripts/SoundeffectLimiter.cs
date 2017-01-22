using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundeffectLimiter : MonoBehaviour {


    static int sounds; 
    public int maxSounds;

    AudioSource audioSource;

	// Use this for initialization
	void OnEnable () {
        audioSource = GetComponent<AudioSource>();
        if (sounds > maxSounds) {
            audioSource.enabled = false;
            enabled = false;
        } else {
            sounds++;
        }
	}

    void Update ()
    {
        if(audioSource.isPlaying == false) {
            sounds--;
            enabled = false;
        }
    }
}
