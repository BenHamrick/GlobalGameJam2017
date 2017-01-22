using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundeffectLimiter : MonoBehaviour {


    static Dictionary<int, int> sounds; 
    public int maxSounds;
    public int id;
    AudioSource audioSource;

	// Use this for initialization
	void OnEnable () {
        if (sounds == null) {
            sounds = new Dictionary<int, int>();
        }
        audioSource = GetComponent<AudioSource>();
        if (sounds.ContainsKey(id) == false) {
            sounds.Add(id, 0);
        }
        if (sounds[id] > maxSounds) {
            audioSource.enabled = false;
            enabled = false;
        } else {
            sounds[id]++;
        }
	}

    void Update ()
    {
        if(audioSource.isPlaying == false) {
            sounds[id]--;
            enabled = false;
        }
    }
}
