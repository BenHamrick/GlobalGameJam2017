using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEngine.SceneManagement;

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
    public static GameManager instance;

    public GameState gameState;
    public GameObject playerPrefab;
    public Transform[] positions;
    List<PlayerController> players;
    public Text startText;
    public Transform startGameObject;
    public Text winningText;
    public GameObject winning;
    public GameObject pressStart;
    public Slider slider;
    public float winningScore;
    float score;
    int positionIndex = 0;

    float startingTime = 0f;

    void Awake()
    {
        pressStart.SetActive(true);
        instance = this;
        score = winningScore / 2f;
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
        if (players.Count > 0 && players[0].inputcontroller.playerActions.Device != null) {
            startingTime += Time.deltaTime;
            if (players[0].inputcontroller.playerActions.Start.WasPressed && startingTime > 1f) {
                gameState = GameState.startingGame;
                startingTime = 5f;
                startGameObject.gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < InputManager.Devices.Count; i++) {
            if (InputManager.Devices[i].CommandWasPressed || InputManager.Devices[i].RightStickButton.WasPressed) {
                StartWasPressed(InputManager.Devices[i]);
            }
        }
    }

    void startingGame()
    {
        pressStart.SetActive(false);
        startingTime -= Time.deltaTime;
        if (startingTime < .1f) {
            startText.text = "GO";
        } else {
            startText.text = "" + Mathf.RoundToInt(startingTime);
        }
        if (startingTime < -.5f) {
            gameState = GameState.gamePlay;
            startGameObject.gameObject.SetActive(false);
        }
        startGameObject.localScale = new Vector2(startingTime % 1f + 1f, startingTime % 1f + 1f);
    }

    void gamePlay()
    {

    }

    void endOfGame()
    {
        startingTime += Time.deltaTime;
        if (slider.value == 1) {
            winningText.text = "Red Team Loses";
        } else {
            winningText.text = "Blue Team Loses";
        }
        if (startingTime > 5f) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
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

    public void Score(Team team)
    {
        if (gameState != GameState.gamePlay) {
            return;
        }
        if (team == Team.blue) {
            score--;
        } else {
            score++;
        }
        slider.value = score / winningScore;
        if (slider.value == 1 || slider.value == 0) {
            gameState = GameState.endOfGame;
            startingTime = 0f;
            winning.SetActive(true);
        }
    }
}
