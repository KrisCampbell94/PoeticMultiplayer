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
            PlayerCanvas.playerCanvas.ToggleHUD(true);
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
            PlayerCanvas.playerCanvas.ToggleHUD(false);
            PlayerCanvas.playerCanvas.ToggleWin(true);
        }
    }

	[ClientRpc]
	public void RpcPlayerLose() {
		if (isLocalPlayer) {
            PlayerCanvas.playerCanvas.ToggleHUD(false);
            PlayerCanvas.playerCanvas.ToggleLose(false);
        }
    }
}
