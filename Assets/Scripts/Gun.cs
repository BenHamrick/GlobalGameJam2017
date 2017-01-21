using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour 
{
	#region Attributes
	[System.Serializable]
	public class WaveAmmo
	{
		#region Attributes
		public GameObject wave;// Holds the actual wave gameobject that will be spawned
		public Wave.WaveType wType;// holds what kind of wave the ammo is
		public int maxAmmoCount;// holds the maximum amount of ammo you can have for this wave
		public int currentAmmoCount;// holds how much ammo is currently left of this wave
		private bool isOnCoolDown;// Holds whether this ammo is on cooldoon or not
		#endregion

		#region Proporties
		public bool IsOnCoolDown
		{
			get{
				return isOnCoolDown;
			}
		}

		public int CurrentAmmoCount
		{
			get{
				return currentAmmoCount;
			}
			set{
				// Go on cooldown if the ammo count reachs or goes below 0
				currentAmmoCount = value;
				isOnCoolDown = (currentAmmoCount <= 0) ? true : false;
				if(isOnCoolDown){
					currentAmmoCount = 0;
				}
			}
		}
		#endregion

		/// <summary>
		/// Should instantiate the wave infront of the gun
		/// </summary>
		/// <param name="gun">Gun.</param>
		public void SpawnWave(GameObject gun, Team team)
		{
			if(!isOnCoolDown)// Make sure we aren't on cooldown
			{
				CurrentAmmoCount--;
				GameObject w = GameObject.Instantiate(wave, gun.transform.position + gun.transform.forward, Quaternion.identity);
				w.GetComponent<Wave>().SetTeam(team);
			}
		}

		/// <summary>
		/// Should refill the ammo count to max
		/// </summary>
		public void RefillAmmo()
		{
			CurrentAmmoCount = maxAmmoCount;
		}
	}
		
	public WaveAmmo[] waves;// Should hold all the types of waves the gun can shot
	public WaveAmmo currentWave;// Holds the current wave teh gun can shot
	public bool isFiring;// holds whether the player is trying to fire the gun or not
	public float fireRate;// Holds how fast the gun can shot waves
	public float coolDownTime;// holds the amount of time that each wave ammo can be on cooldown for
	private Team team;
	#endregion

	#region Initializing
	void Start()
	{
		team = GetComponent<PlayerController>().team;
	}
	#endregion

	#region FireWave
	public void FireWave1()
	{
		currentWave = waves[0];
		StartWave();
	}

	public void FireWave2()
	{
		currentWave = waves[1];
		StartWave();
	}

	public void FireWave3()
	{
		currentWave = waves[2];
		StartWave();
	}

	void StartWave()
	{
		if(!isFiring){
			StartCoroutine(FireWave(currentWave));
		}
	}

	void EndWave()
	{
		StopCoroutine("SpawnWave");
	}

	IEnumerator FireWave(WaveAmmo wAmmo)
	{
		isFiring = true;
		while(!wAmmo.IsOnCoolDown && isFiring)
		{
			wAmmo.SpawnWave(gameObject, team);
			if(wAmmo.IsOnCoolDown){
				StartCoroutine(CoolDown(wAmmo));
			}
			yield return new WaitForSeconds(fireRate);
		}
	}
	#endregion

	#region CoolDown
	IEnumerator CoolDown(WaveAmmo wAmmo)
	{
		yield return new WaitForSeconds(coolDownTime);
		wAmmo.RefillAmmo();
	}
	#endregion
}
