using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using static NPCController;

public class PlayerAllyController : NetworkBehaviour
{
	public GameObject npcPrefab;

	public int numAlliesAtStart = 2;

	private GameObject lastAlly;

	private Color color;

	public int allyCount { get; private set; }

	// Start is called before the first frame update
	void Start() {
		CmdInitColor();

		if (this.isLocalPlayer) {
			InitAllies();
		}
	}

	[Command]
	void CmdInitColor() {
		color = ColorManager.globalInstance.AssignColor();
		transform.Find("Body").GetComponent<MeshRenderer>().material.color = color;
	}

	void InitAllies() {
		Vector3 pos = this.transform.position;
		pos.z -= 5;

		for (int i = 1; i <= numAlliesAtStart; i++) {
			GameObject ally = Instantiate(npcPrefab, pos, this.transform.rotation);
			NetworkServer.Spawn(ally);
			AddAlly(ally);
		}
	}

	public void AddAlly(GameObject ally) {
		NPCController allyController = ally.GetComponent<NPCController>();

		// If have any ally,
		if (lastAlly != null) {
			// Set as follower of last ally
			NPCController lastAllyController = lastAlly.GetComponent<NPCController>();
			lastAllyController.follower = allyController;
			allyController.leader = lastAlly.transform;
		} else {
			// Set as follower of self
			allyController.leader = this.transform;
		}

		allyController.state = NPCState.Follow; // Set ally state

		ally.GetComponent<NavMeshAgent>().speed = 16; // Set ally speed

		allyController.visorMeshRenderer.material.color = color; // Set ally color

		allyController.playerAllyController = this; // Add reference self
		lastAlly = ally; // Store reference to last ally
		allyCount++; // Keep track of total allies
	}

	public void RemoveAlly(GameObject ally) {
		NPCController allyController = ally.GetComponent<NPCController>();

		if (allyController.playerAllyController == this) // Ally of this player
		{
			if (ally == lastAlly) // Is last ally
			{
				if (allyCount > 1) {
					// Set leader as last ally instead of self
					lastAlly = allyController.leader.gameObject;
				} else {
					// Lost all allies, do lose condition
				}
			} else if (allyController.follower != null) // Is not last and has follower
			{
				allyController.follower.leader = allyController.leader; // Set new leader for follower
			}

			allyController.leader = null; // Set no leader for self

			allyController.state = NPCState.Patrol; // Set ally state

			ally.GetComponent<NavMeshAgent>().speed = 8; // Set ally speed

			allyController.playerAllyController = null; // Remove reference to self

			allyCount--; // Keep track of total allies
		}
	}

	public void OnHitNPC(GameObject npc) {
		NPCController npcController = npc.GetComponent<NPCController>();

		if (npcController.playerAllyController == null) { // Not allied
			AddAlly(npc); // Add to self allies
		} else if (npcController.playerAllyController != this) { // Allied but not with self
			npcController.playerAllyController.RemoveAlly(npc); // Remove from other player
		}
	}
}
