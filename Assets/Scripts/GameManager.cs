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

	private List<GameObject> players;
	public int npcCount = 0;
	public bool npcSpawningComplete = false;

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

	public void PlayerJoin(GameObject player) {
		players.Add(player);
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
		Debug.Log(count + " " + GetAlliesToWin());
		if (npcSpawningComplete && (count >= GetAlliesToWin())) {
			HaveEnoughAlliesToWin(player);
		}
	}

	//

	public int GetAlliesToWin() {
		float multiplier = percentAlliesToWin / 100f;
		return Mathf.RoundToInt(npcCount * multiplier);
	}
}
