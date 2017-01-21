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
		#endregion
	
		/// <summary>
		/// Should instantiate the wave infront of the gun
		/// </summary>
		/// <param name="gun">Gun.</param>
		public void SpawnWave(Gun gun, Team team)
		{
			Vector3 positionAdjuster = (team == Team.blue) ? gun.waveSpawnPostionAdjuster : gun.waveSpawnPostionAdjuster * -1;
			GameObject w = GameObject.Instantiate(wave, gun.gameObject.transform.position + gun.gameObject.transform.forward + positionAdjuster , Quaternion.identity);
			w.GetComponent<Wave>().SetTeam(team);
		}
	}
		
	public WaveAmmo[] waves;// Should hold all the types of waves the gun can shot
	public WaveAmmo currentWave;// Holds the current wave teh gun can shot
	public bool isFiring;// holds whether the player is trying to fire the gun or not
	public float fireRate;// Holds how fast the gun can shot waves
    public int maxAmmoCount;// holds the maximum amount of ammo you can have for this wave
    public int currentAmmoCount;// holds how much ammo is currently left of this wave
    public bool alreadyRefilling;// holds whether there is already a coroutine that is refilling the ammo of this wave
    private bool isOutOfAmmo;// Holds whether there is any ammo left or not
    public float ammoRefillRate;// holds the amount of time in seconds it takes for one 1
	public Vector3 waveSpawnPostionAdjuster;// This adjust the position that the waves spawn at
	private Team team; // holds which team the player using this gun is from
    #endregion

    #region Properties
    public bool IsOutOfAmmo 
    {
        get {
            return isOutOfAmmo = currentAmmoCount <= 0;
        }
    }
    #endregion

    #region Initializing
    void Start()
	{
        currentAmmoCount = maxAmmoCount;
		team = GetComponent<PlayerController>().team;
	}
	#endregion

	#region Fire Wave
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

	IEnumerator FireWave(WaveAmmo wAmmo)
	{
		isFiring = true;
        if (!IsOutOfAmmo) {
            wAmmo.SpawnWave(this, team);
            currentAmmoCount--;
            if (currentAmmoCount != maxAmmoCount && !alreadyRefilling) {
                StartCoroutine(RefillingAmmo());
            }

            yield return new WaitForSeconds(fireRate);
        }
        isFiring = false;
	}
	#endregion

	#region Refilling 
	IEnumerator RefillingAmmo()
	{
		alreadyRefilling = true;
		while(currentAmmoCount != maxAmmoCount)
		{
			yield return new WaitForSeconds(ammoRefillRate);
			RefillAmmo();
		}
		alreadyRefilling = false;
	}

    /// <summary>
    /// Should refill the ammo count to max
    /// </summary>
    public void RefillAmmo()
    {
        currentAmmoCount++;
    }
    #endregion
}
