using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour 
{
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
				if(value <= 0)
				{
					isOnCoolDown = true;
				}
				else
				{
					isOnCoolDown = false;
					currentAmmoCount = value;
				}
			}
		}
		#endregion

		/// <summary>
		/// Should instantiate the wave infront of the gun
		/// </summary>
		/// <param name="gun">Gun.</param>
		public void FireWave(GameObject gun)
		{
			if(!isOnCoolDown)// Make sure we aren't on cooldown
			{
				currentAmmoCount--;
				GameObject.Instantiate(wave, gun.transform.forward, Quaternion.identity);
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

	#region Attributes
	public WaveAmmo[] waves;// Should hold all the types of waves the gun can shot
	public WaveAmmo currentWave;// Holds the current wave teh gun can shot
	public float fireRate;// Holds how fast the gun can shot waves
	public float coolDownTime;// holds the amount of time that each wave ammo can be on cooldown for
	#endregion

	/// <summary>
	/// Changes the current wave the gun can shot
	/// </summary>
	#region Change Wave Type
	public void ChangeWaveTypeToWave1()
	{
		currentWave = waves[0];
	}

	public void ChangeWaveTypeToWave2()
	{
		currentWave = waves[1];
	}

	public void ChangeWaveTypeToWave3()
	{
		currentWave = waves[2];
	}
	#endregion

	#region Fire Wave
	void StartWave()
	{
		StartCoroutine(SpawnWave(currentWave));
	}

	void EndWave()
	{
		StopCoroutine("SpawnWave");
	}

	IEnumerator CooolDown(WaveAmmo wAmmo)
	{
		yield return new WaitForSeconds(coolDownTime);
		wAmmo.RefillAmmo();
	}

	IEnumerator SpawnWave(WaveAmmo wAmmo)
	{
		while(!wAmmo.IsOnCoolDown)
		{
			wAmmo.FireWave(gameObject);
			yield return new WaitForSeconds(fireRate);
		}
	}
	#endregion
		
	// Update is called once per frame
	void Update () {
		
	}
}
