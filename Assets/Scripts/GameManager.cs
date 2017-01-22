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

    public int colorIndex;

    public AudioClip[] countDown;
    public AudioSource countDownAudioSource;

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
    public BeatDetection beatDetection;
    float score;
    int positionIndex = 0;

    float startingTime = 0f;
    int countDownIndex = 0;

	public GameObject[] deathLazers;
    public List<MusicTextSync> textSync;
    public List<MusicHueSync> hueSync;

    void Awake()
    {
        beatDetection = KeepAlive.instance.GetComponent<BeatDetection>();
        textSync = new List<MusicTextSync>();
        hueSync = new List<MusicHueSync>();
        pressStart.SetActive(true);
        instance = this;
        score = winningScore / 2f;
        players = new List<PlayerController>(); 

        beatDetection.CallBackFunction = MyCallbackEventHandler;
    }

    public void MyCallbackEventHandler(BeatDetection.EventInfo eventInfo) {
        for (int i = 0; i < textSync.Count; i++) {
            textSync[i].RandomColor();
        }
        for (int i = 0; i < hueSync.Count; i++) {
            hueSync[i].Randomize();
        }
        Debug.Log(eventInfo.messageInfo);
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
        if (players.Count > 0) {
            startingTime += Time.deltaTime;
            if (players[0].inputcontroller.playerActions.Start.WasPressed && startingTime > 1f) {
                gameState = GameState.startingGame;
                startingTime = 5f;
                countDownIndex = 6;
                startGameObject.gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < InputManager.Devices.Count; i++) {
            if (InputManager.Devices[i].CommandWasPressed || InputManager.Devices[i].RightStickButton.WasPressed) {
                StartWasPressed(InputManager.Devices[i]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            KeyboardZWasPressed();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            KeyboardUWasPressed();
        }
    }

    void startingGame()
    {
        pressStart.SetActive(false);
        startingTime -= Time.deltaTime;
        if (startingTime < .5f) {
            startText.text = "GO";
        } else {
            startText.text = "" + Mathf.RoundToInt(startingTime);
        }
        if (countDownIndex > Mathf.RoundToInt(startingTime) && countDownIndex > 0) {
            countDownAudioSource.clip = countDown[countDownIndex - 1];
            countDownAudioSource.Play();
            countDownIndex--;
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
        if (startingTime > 5f) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

	void killTeam(Team team)
	{
		for(int i = 0; i < players.Count; i++)
		{
			if(players[i].team == team)
				players[i].KillPlayer(regularDeath:false);
		}
	}

    void KeyboardUWasPressed()
    {
        for (int i = 0; i < players.Count; i++) {
            if (players[i].id == 3) {
                return;
            }
        }
        GameObject player = (GameObject)Instantiate(playerPrefab, positions[positionIndex].position, Quaternion.identity);
        positionIndex++;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.id = 3;
        if (positionIndex % 2 == 0) {
            playerController.team = Team.blue;
        }
        else {
            playerController.team = Team.red;
        }

        playerController.inputcontroller.playerActions = PlayerActions.CreateWithPlayer2KeyboardBindings();
        players.Add(playerController);
    }

    void KeyboardZWasPressed()
    {
        for (int i = 0; i < players.Count; i++) {
            if (players[i].id == 2) {
                return;
            }
        }
        GameObject player = (GameObject)Instantiate(playerPrefab, positions[positionIndex].position, Quaternion.identity);
        positionIndex++;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.id = 2;
        if (positionIndex % 2 == 0) {
            playerController.team = Team.blue;
        } else {
            playerController.team = Team.red;
        }
        
        playerController.inputcontroller.playerActions = PlayerActions.CreateWithPlayer1KeyboardBindings();
        players.Add(playerController);
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
        }
        else {
            playerController.team = Team.red;
        }

        playerController.inputcontroller.playerActions = PlayerActions.CreateControllerBindings();
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

			if (slider.value == 1) {
				winningText.text = "Orange Team Loses";
				deathLazers[0].SetActive(true);
				killTeam(Team.red);
			} else {
				winningText.text = "Blue Team Loses";
				deathLazers[1].SetActive(true);
				killTeam(Team.blue);
			}
        }
    }

	public IEnumerator Respawn(PlayerController player)
	{
		yield return new WaitForSeconds(5f);
		bool positionFound = false;
		while(!positionFound && gameState != GameState.endOfGame)
		{
			int positionIndex = Random.Range(0, positions.Length - 1);
			if(player.team == Team.blue && positionIndex % 2 != 0){
				positionFound = true;
				player.gameObject.transform.position = positions[positionIndex].position;
			}
			else if(player.team == Team.red && positionIndex % 2 == 0)
			{
				positionFound = true;
				player.gameObject.transform.position = positions[positionIndex].position;
			}
		}

		if(gameState != GameState.endOfGame){
            player.RevivePlayer();
        }
	}
}
