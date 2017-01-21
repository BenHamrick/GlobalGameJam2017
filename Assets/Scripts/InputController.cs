using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public PlayerActions playerActions;
	public Gun playerGun;
    PlayerController playerController;

    // Use this for initialization
    void Start () {
        playerController = GetComponent<PlayerController>();
        if (playerActions == null) {
            playerActions = PlayerActions.CreateWithDebugBindings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerActions.Start.WasPressed) {
        //    GameManager.instance.menuController.PauseToggle();
        //}

        if (Time.timeScale == 0f) {
            return;
        }
        Vector2 direction = playerActions.Move.Vector;
        if (direction.magnitude > .1f) {
            playerController.Move(direction);
        }
        else {
            playerController.Stop();
        }

        if (playerActions.Jump.WasPressed) {
            if (playerController.isOnGround) {
                playerController.Jump();
            }
        }

		if(playerActions.Wave1.IsPressed){
			playerGun.FireWave1();
		}
		else if(playerActions.Wave2.IsPressed){
			playerGun.FireWave2();
		}
		else if(playerActions.Wave3.IsPressed){
			playerGun.FireWave3();
		}
		else{
			playerGun.isFiring = false;
		}
    }
}
