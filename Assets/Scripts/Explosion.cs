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
