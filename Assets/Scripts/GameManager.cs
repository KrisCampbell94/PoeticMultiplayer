using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
	public static GameManager globalInstance;

	public bool loseOnLostAllAllies = true;

	public bool winOnHaveEnoughAllies = true;
	public int percentAlliesToWin = 50;

	public bool winOnTimer = true;
	public float secondsPerGame = 120;

	public bool addTimeOnTie = true;
	public float secondsPerOvertime = 5;

	//

	private List<GameObject> players;
	public int npcCount = 0;
	public bool npcSpawningComplete = false;

	private bool gameStarted = false;
	private float timeRemaining = 0;
	public string timeRemainingStr = "00:00";

	// Start is called before the first frame update
	void Start() {
		if (globalInstance == null) {
			globalInstance = this;
			Init();
		} else if (globalInstance != this) {
			Debug.LogError("Multiple instances of GameManager found");
		}
	}

	public void Init() {
		players = new List<GameObject>();
	}

	private void Update() {
		if (isServer) {
			if (winOnTimer && gameStarted) {
				timeRemaining -= Time.deltaTime;
				timeRemaining = Mathf.Max(0, timeRemaining);
				UpdateTimeRemainingStr();

				if (timeRemaining <= 0) { // Game should be over now,
					// Get most ally count
					int mostAllies = 0;
					foreach (GameObject player in players) {
						int thisPlayerAllyCount = player.GetComponent<PlayerAllyController>().allyCount;
						if (thisPlayerAllyCount > mostAllies) { // This player has more than previous max
							mostAllies = thisPlayerAllyCount; // Set new max
						}
					}

					// Remove anyone who doesn't have that many allies
					foreach (GameObject player in players) {
						int thisPlayerAllyCount = player.GetComponent<PlayerAllyController>().allyCount;
						if (thisPlayerAllyCount < mostAllies) {
							PlayerLose(player);
						}
					}

					// If there is a tie, add overtime
					if (players.Count > 1) {
						timeRemaining += secondsPerOvertime;
					}
				}
			}
		}
	}

	public void StartGame() {
		timeRemaining = secondsPerGame;
		gameStarted = true;
	}

	//

	public void PlayerJoin(GameObject player) {
		players.Add(player);

		if (players.Count >= 2) {
			StartGame();
		}
	}

	public void PlayerLose(GameObject player) {
		// Remove player and set to lose
		players.Remove(player);
		player.GetComponent<PlayerWinController>().RpcPlayerLose();

		// If 1 player left, set that player to win
		if (players.Count == 1) {
			PlayerWin(players[0]);
		}
	}

	public void PlayerWin(GameObject player) {
		player.GetComponent<PlayerWinController>().RpcPlayerWin();
	}

	//

	public void LostAllAllies(GameObject player) {
		if (loseOnLostAllAllies) {
			PlayerLose(player);
		}
	}

	public void HaveEnoughAlliesToWin(GameObject player) {
		if (winOnHaveEnoughAllies) {
			PlayerWin(player);
		}
	}

	public void GotAlly(GameObject player, int count) {
		if (npcSpawningComplete && (count >= GetAlliesToWin())) {
			HaveEnoughAlliesToWin(player);
		}
	}

	//

	public int GetAlliesToWin() {
		float multiplier = percentAlliesToWin / 100f;
		return Mathf.RoundToInt(npcCount * multiplier);
	}

	private void UpdateTimeRemainingStr() {
		// https://answers.unity.com/questions/45676/making-a-timer-0000-minutes-and-seconds.html
		string minutes = Mathf.Floor(timeRemaining / 60).ToString("00");
		string seconds = Mathf.RoundToInt(timeRemaining % 60).ToString("00");

		timeRemainingStr = $"{minutes}:{seconds}";
	}
}
