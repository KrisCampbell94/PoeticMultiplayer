using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerWinController : NetworkBehaviour
{
	private void Start() {
		if (this.isLocalPlayer) {
			CmdInitPlayer();
		}
	}

	[Command]
	void CmdInitPlayer() {
		// Tell GameManager to add this player
		GameManager.globalInstance.PlayerJoin(this.gameObject);
	}

	[ClientRpc]
	public void RpcPlayerWin() {
		if (isLocalPlayer) {
			SceneManager.LoadScene("Win");
		}
	}

	[ClientRpc]
	public void RpcPlayerLose() {
		if (isLocalPlayer) {
			SceneManager.LoadScene("Lose");
		}
	}
}
