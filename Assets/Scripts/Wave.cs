﻿using System.Collections;
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
	public float damage;// holds the amount of damge the wave can do to a player
	public GameObject explosionEffect;// Holds the prefab of a particle effect that is spawned when the wave collideds with another
	public GameObject scoreEffect;// Holds the particle effect that will be spawned when the wave collideds with the wall of the other team
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
        transform.localScale = new Vector3((team == Team.blue) ? Mathf.Abs(transform.localScale.x) *-1 : Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}
	#endregion

	#region Collision
	/// <summary>
	/// Should be called if a wave ever enters the trigger of another wave
	/// </summary>
	/// <param name="collider">Collider.</param>
	void WaveCollision(Collider2D collider)
	{
		Wave otherWave = collider.gameObject.GetComponent<Wave>();
		if(otherWave.team != team && otherWave.wType == wType)
		{
			Vector3 midPoint = transform.position + ((otherWave.transform.position - transform.position) * .5f);
            GameObject exEffect = NewSpawnPool.instance.getGameObject(explosionEffect);
            exEffect.transform.position = midPoint;
			//exEffect.AddComponent<Explosion>();
			//Destroy(exEffect.GetComponent<DemoReactivator>());

			Debug.Log("Collided with wave: ");
            gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Should be called if a player ever enters the trigger of the wave
	/// </summary>
	void PlayerCollision(Collider2D collider)
	{
		PlayerController player = collider.gameObject.GetComponent<PlayerController>();
		if(player.team != team && player.Health > 0)
		{
			player.damagePlayer(damage);
			Debug.Log("Collided with player: ");
            gameObject.SetActive(false);
        }
	}

	void WallCollision(Collider2D collider)
	{
		Debug.Log("Collided with wall: ");
        GameObject scEffect = NewSpawnPool.instance.getGameObject(scoreEffect);
        scEffect.transform.position = transform.position;

  //      scEffect.AddComponent<Explosion>();
		//Destroy(scEffect.GetComponent<DemoReactivator>());

		WallController wController = collider.gameObject.GetComponent<WallController>();
		wController.Score();
        gameObject.SetActive(false);
    }

	void OnTriggerEnter2D(Collider2D collider)
	{
		//Check for collision with waves and players
		if(collider.gameObject.tag == "Wave")
		{
			WaveCollision(collider);
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
