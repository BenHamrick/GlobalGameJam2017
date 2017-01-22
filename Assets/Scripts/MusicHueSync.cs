using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHueSync : MonoBehaviour {

    public float[] hues;

    int index;
    _2dxFX_ColorChange colorChange;

	// Use this for initialization
	void Start () {
        colorChange = GetComponent<_2dxFX_ColorChange>();
        GameManager.instance.hueSync.Add(this);
    }
	
	public void Randomize()
    {
        int random = index;
        while (random == index) {
            random = Random.Range(0, hues.Length);
        }
        colorChange._HueShift = hues[random];
    }
}
