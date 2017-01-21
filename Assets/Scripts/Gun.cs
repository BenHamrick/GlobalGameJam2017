using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour 
{
	public class WaveAmmo
	{
		#region Attributes
		public GameObject wave;
		public Wave.WaveType wType;
		public int ammoCount;
		private bool isOnCoolDown;
		private float ammoRefillRate;
		#endregion
	}

	#region Attributes
	public WaveAmmo[] waves;
	public float fireRate;
	#endregion
	// Use this for initialization
	void Start () {
		
	}

	void StartWave()
	{
		StartCoroutine(SpawnWave());
	}

	void EndWave()
	{
		
	}

	IEnumerator SpawnWave()
	{
		return null;
	}

	IEnumerator CoolDown()
	{
		return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
