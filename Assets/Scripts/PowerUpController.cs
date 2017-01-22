using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

    public GameObject healthPowerup;
    public GameObject energyPowerup;

    float requieredHealthTime;
    float requieredEnergyTime;

    float healthTimer;
    float energyTimer;

	// Use this for initialization
	void Start () {
        requieredHealthTime += 10f;
        requieredEnergyTime += 15f;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.gameState != GameState.gamePlay) {
            return;
        }
        if (healthPowerup.activeInHierarchy == false) {
            healthTimer += Time.deltaTime;
        }
        if (energyPowerup.activeInHierarchy == false) {
            energyTimer += Time.deltaTime;
        }

        if (healthTimer > requieredHealthTime) {
            requieredHealthTime += 30f;
            healthPowerup.SetActive(true);
        }

        if (energyTimer > requieredEnergyTime) {
            requieredEnergyTime += 30f;
            energyPowerup.SetActive(true);
        }
    }
}
