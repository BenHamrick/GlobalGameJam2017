using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveExplosion : MonoBehaviour 
{
	#region Attributes
	public float secondsBeforeObjectDestroyed = 3f;
	#endregion

	#region Destroy Explosion
	void Start()
	{
		StartCoroutine(DestroyTimer());
	}

	IEnumerator DestroyTimer()
	{
		yield return new WaitForSeconds(secondsBeforeObjectDestroyed);
		Destroy(gameObject);
	}
	#endregion
}
