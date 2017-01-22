using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundeffectLimiter : MonoBehaviour {


    static Dictionary<int, int> sounds; 
    public int skipSounds;
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
        if (sounds[id] > skipSounds) {
            sounds[id] = 0;
        } else {
            sounds[id]++;
            audioSource.enabled = false;
            enabled = false;
        }
	}
}
