using UnityEngine;
using System.Collections;

public class SplashScreenListener : MonoBehaviour {

    public string nextScene;

	public void FinishedAnimation(){
        NiceSceneTransition.instance.LoadScene(nextScene);
	}
}
