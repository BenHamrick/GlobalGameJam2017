using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAlive : MonoBehaviour {

    public static KeepAlive instance = null;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(transform.gameObject);
        }
    }
}
