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
	/// <summary>
	/// Holds the direction this wave will move towards
	/// </summary>
	private Vector2 direction;
	#endregion

	#region Movement
	// Update is called once per frame
	void Update ()
	{
		MoveWave();
	}
		
	/// <summary>
	/// Should set the direction the wave will be moving, this should be set by the gun
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void SetDirection(Vector2 direction)
	{
		this.direction = direction;
	}

	/// <summary>
	/// This will move the wave in it's chosen direction
	/// </summary>
	void MoveWave()
	{
		transform.position += (Vector3)direction * (movementSpeed * Time.deltaTime);
	}
	#endregion

	#region Collision
	/// <summary>
	/// Should be called if a wave ever enters the trigger of another wave
	/// </summary>
	/// <param name="collider">Collider.</param>
	void WaveCollision(Collider collider)
	{
		Debug.Log("Collided with wave: ");
	}

	/// <summary>
	/// Should be called if a player ever enters the trigger of the wave
	/// </summary>
	void PlayerCollision(Collider collider)
	{
		Debug.Log("Collided with player: ");
	}

	void OnTriggerEnter(Collider collider)
	{
		//Check for collision with waves and players
		if(collider.gameObject.tag == "Wave")
		{
			WaveCollision(collider);
		}
		else if(collider.gameObject.tag == "Player")
		{
			PlayerCollision(collider);
		}
	}
	#endregion
}
