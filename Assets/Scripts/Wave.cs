using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour 
{
	#region Attributes
	/// <summary>
	/// Contains all of the types of waves there can be
	/// </summary>
	public enum WaveType
	{
		Whirly,
		Boxy,
		Diagnoly
	}

	/// <summary>
	/// Holds the type this wave is
	/// </summary>
	public WaveType wType;
	/// <summary>
	/// Holds the speed in which the wave will move
	/// </summary>
	public float movementSpeed;
	public GameObject explosionEffect;
	private Team team;// Holds the team this wave belongs to
	private Vector3 direction;// Holds the direction this wave will move in
	#endregion

	#region Movement
	// Update is called once per frame
	void Update ()
	{
		MoveWave();
	}

	/// <summary>
	/// This will move the wave in it's chosen direction
	/// </summary>
	void MoveWave()
	{
		transform.position += direction * (movementSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Should set the team that shot this wave
	/// </summary>
	/// <param name="team">Team.</param>
	public void SetTeam(Team team)
	{
		this.team = team;
		direction = (team == Team.blue) ? Vector3.right : Vector3.left;
	}
	#endregion

	#region Collision
	/// <summary>
	/// Should be called if a wave ever enters the trigger of another wave
	/// </summary>
	/// <param name="collider">Collider.</param>
	void WaveCollision(Team otherTeam, WaveType otherWType)
	{
		if(otherTeam != team && otherWType == wType)
		{
			RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction);
			if(hit.collider != null){
				GameObject exEffect = GameObject.Instantiate(explosionEffect, (Vector3)hit.point,Quaternion.identity);
				exEffect.AddComponent<WaveExplosion>();
				Destroy(exEffect.GetComponent<DemoReactivator>());
			}

			Debug.Log("Collided with wave: ");
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Should be called if a player ever enters the trigger of the wave
	/// </summary>
	void PlayerCollision(Collider2D collider)
	{
		PlayerController player = collider.gameObject.GetComponent<PlayerController>();
		if(player.team != team)
		{
			Debug.Log("Collided with player: ");
			Destroy(gameObject);
		}
	}

	void WallCollision(Collider2D collider)
	{
		Debug.Log("Collided with wall: ");
		WallController wController = collider.gameObject.GetComponent<WallController>();
		wController.Score();
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		//Check for collision with waves and players
		if(collider.gameObject.tag == "Wave")
		{
			Wave wave = collider.gameObject.GetComponent<Wave>();
			WaveCollision(wave.team, wave.wType);
			Destroy(wave);
		}
		else if(collider.gameObject.tag == "Wall")
		{
			WallCollision(collider);
		}
		else if(collider.gameObject.tag == "Player")
		{
			PlayerCollision(collider);
		}

	}
	#endregion
}
