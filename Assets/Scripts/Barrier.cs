using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour 
{
	#region Attributes
	public GameObject thunda;
	#endregion

	#region The Thunda
	IEnumerator BringDownTheThunda(PlayerController player)
	{
		GameObject thunder = GameObject.Instantiate(thunda, player.transform.position + (Vector3.down * 1.3f), thunda.transform.rotation);
		thunder.AddComponent<Explosion>();

		yield return new WaitForSeconds(0F);
		player.KillPlayer();
	}
	#endregion

	#region Collision
	void OnTriggerEnter2D(Collider2D collider)
	{
		PlayerController player = collider.gameObject.GetComponent<PlayerController>();
		if(player != null)
		{
			StartCoroutine(BringDownTheThunda(player));
		}
	}
	#endregion
}
