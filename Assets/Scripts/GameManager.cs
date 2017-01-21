using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public enum Team
{
    red,
    blue
}

public class GameManager : MonoBehaviour {
    public GameObject playerPrefab;
    public Transform[] positions;
    List<PlayerController> players;
    int positionIndex = 0;

    void Awake()
    {
        players = new List<PlayerController>();
    }

    void Update () {
        for (int i = 0; i < InputManager.Devices.Count; i++) {
            if (InputManager.Devices[i].CommandWasPressed || InputManager.Devices[i].RightStickButton.WasPressed) {
                StartWasPressed(InputManager.Devices[i]);
            }
        }
    }

    void StartWasPressed(InputDevice device)
    {
        for (int i = 0; i < players.Count; i++) {
            if (players[i].inputcontroller.playerActions.Device != null &&
                players[i].inputcontroller.playerActions.Device == device) {
                return;
            }
        }
        GameObject player = (GameObject)Instantiate(playerPrefab, positions[positionIndex].position, Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.inputcontroller.playerActions = PlayerActions.CreateWithDebugBindings();
        playerController.inputcontroller.playerActions.Device = device;
        players.Add(playerController);
    }
}
