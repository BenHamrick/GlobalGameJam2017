using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour 
{
	#region Attributes
	public float secondsBeforeObjectDestroyed = 3f;
	#endregion

	#region Destroy Explosion
	void OnEnable()
	{
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.enabled) {
            audioSource.Play();
        }
		StartCoroutine(DestroyTimer());
	}

	IEnumerator DestroyTimer()
	{
		yield return new WaitForSeconds(secondsBeforeObjectDestroyed);
        //Destroy(gameObject);
        gameObject.SetActive(false);
	}
	#endregion
}
