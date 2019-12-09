using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
	public const int maxHealth = 100;
	//step 13 adding (hook=...)

	[SyncVar(hook="OnChangeHealth")]
	public int currentHealth=maxHealth; //syncrynized on all clients

	public RectTransform healthBar;

	//step 16
	public bool destroyOnDeath;
	// Use this for initialization


	//step 17
	private NetworkStartPosition[] spawnPositions;
	void Start () {
		if (isLocalPlayer) {
			spawnPositions = FindObjectsOfType<NetworkStartPosition> ();
			// Debug.Log ("spawnPositions.length=" + spawnPositions.Length);

		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(int amount){
		if (!this.isServer)
			return;
		currentHealth -= amount;
		if (currentHealth <= 0) {
			//The following 2 lines were active in steps <= 13
			//currentHealth = maxHealth;
			//Debug.Log ("PlayerId:" + this.GetInstanceID () + " is dead!"); 


			//step 16
			if (destroyOnDeath) {
				Destroy (this.gameObject);
			} else {

				//Step 14
				//The following 2 lines are active in steps 14+ 
				currentHealth = maxHealth;
				RpcRespawn ();
			}
		}


	}
	//step 13
	void OnChangeHealth(int health){
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	//step 14
	[ClientRpc]
	void RpcRespawn(){
		if (isLocalPlayer) {
			Vector3 spawnPos = Vector3.zero;
			if (spawnPositions != null && spawnPositions.Length > 0) {
				spawnPos = spawnPositions [Random.Range (0, spawnPositions.Length)].transform.position;
			}
			transform.position = spawnPos;
		}
	}
		
}
