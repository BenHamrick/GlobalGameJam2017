using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public enum Team
{
    red,
    blue
}

public enum GameState
{
    waitingOnPlayers,
    startingGame,
    gamePlay,
    endOfGame
}

public class GameManager : MonoBehaviour {
    public GameState gameState;
    public GameObject playerPrefab;
    public Transform[] positions;
    List<PlayerController> players;
    public Text startText;
    public Transform startGameObject;
    int positionIndex = 0;

    float startingTime = 5f;

    void Awake()
    {
        players = new List<PlayerController>();
    }

    void Update () {
        switch (gameState) {
            case GameState.waitingOnPlayers:
                waitingOnPlayers();
                break;
            case GameState.startingGame:
                startingGame();
                break;
            case GameState.gamePlay:
                gamePlay();
                break;
            case GameState.endOfGame:
                endOfGame();
                break;
            default:
                break;
        }
    }

    void waitingOnPlayers ()
    {
        for (int i = 0; i < InputManager.Devices.Count; i++) {
            if (InputManager.Devices[i].CommandWasPressed || InputManager.Devices[i].RightStickButton.WasPressed) {
                StartWasPressed(InputManager.Devices[i]);
            }
        }
        if (players.Count > 0 && players[0].inputcontroller.playerActions.Device != null) {
            if (players[0].inputcontroller.playerActions.Start.WasPressed) {
                gameState = GameState.startingGame;
                startingTime = 5f;
                startGameObject.gameObject.SetActive(true);
            }
        }
    }

    void startingGame()
    {
        startingTime -= Time.deltaTime;
        startText.text = "" + Mathf.RoundToInt(startingTime / 1000f);
    }

    void gamePlay()
    {

    }

    void endOfGame()
    {

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
        positionIndex++;
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (positionIndex % 2 == 0) {
            playerController.team = Team.blue;
        } else {
            playerController.team = Team.red;
        }
        
        playerController.inputcontroller.playerActions = PlayerActions.CreateWithDebugBindings();
        playerController.inputcontroller.playerActions.Device = device;
        players.Add(playerController);
    }
}
