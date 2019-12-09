using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using static NPCController;

public class PlayerAllyController : NetworkBehaviour
{
	public GameObject npcPrefab;

	public int numAlliesAtStart = 2;

	private GameObject lastAlly;

	[SyncVar]
	public Color color;

	public int allyCount { get; private set; }

	// Start is called before the first frame update
	void Start() {
		if (this.isLocalPlayer) {
			CmdInitPlayer();
			CmdInitAllies();
		} else {
			transform.Find("Body").GetComponent<MeshRenderer>().material.color = color;
		}
	}

	[Command]
	void CmdInitPlayer() {
		// Init player color
		Color color = ColorManager.globalInstance.GetColor();
		this.color = color;
		RpcSetColor(color);
	}

	[ClientRpc]
	void RpcSetColor(Color color) {
		this.color = color;
		transform.Find("Body").GetComponent<MeshRenderer>().material.color = color;
	}

	[Command]
	void CmdInitAllies() {
		Vector3 pos = this.transform.position;
		pos.z -= 5;

		GameManager.globalInstance.npcCount += numAlliesAtStart;
		for (int i = 1; i <= numAlliesAtStart; i++) {
			GameObject ally = Instantiate(npcPrefab, pos, this.transform.rotation);
			NetworkServer.Spawn(ally);
			CmdAddAlly(ally);
		}
    }

	[Command]
	public void CmdAddAlly(GameObject ally) {
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

		allyController.playerAllyController = this; // Add reference self
		lastAlly = ally; // Store reference to last ally
		allyCount++; // Keep track of total allies

		GameManager.globalInstance.GotAlly(this.gameObject, allyCount);

		RpcAddAlly(ally);
	}
	
	[ClientRpc]
	void RpcAddAlly(GameObject ally) {
		NPCController allyController = ally.GetComponent<NPCController>();
		allyController.SetColor(color);
	}

	[Command]
	public void CmdRemoveAlly(GameObject ally) {
		NPCController allyController = ally.GetComponent<NPCController>();

		if (allyController.playerAllyController == this) // Ally of this player
		{
			if (ally == lastAlly) // Is last ally
			{
				if (allyCount > 1) { // Have more than just this one
					// Set leader as last ally instead of self
					lastAlly = allyController.leader.gameObject;
				} else { // This is the last one
					lastAlly = null;
					GameManager.globalInstance.LostAllAllies(this.gameObject);
				}
			} else if (allyController.follower != null) // Is not last, has follower
			{
				allyController.follower.leader = allyController.leader; // Set new leader for follower
			}

			allyController.leader = null; // Set no leader for self

			allyController.state = NPCState.Patrol; // Set ally state

			ally.GetComponent<NavMeshAgent>().speed = 8; // Set ally speed

			allyController.playerAllyController = null; // Remove reference to self

			allyCount--; // Keep track of total allies

			RpcRemoveAlly(ally);
		}
	}

	[ClientRpc]
	void RpcRemoveAlly(GameObject ally) {
		NPCController allyController = ally.GetComponent<NPCController>();
		allyController.SetColor(Color.clear);
	}

	[Command]
	public void CmdOnHitNPC(GameObject npc) {
		NPCController npcController = npc.GetComponent<NPCController>();

		if (npcController.playerAllyController == null) { // Not allied
			CmdAddAlly(npc); // Add to self allies
		} else if (npcController.playerAllyController != this) { // Allied but not with self
			npcController.playerAllyController.CmdRemoveAlly(npc); // Remove from other player
		}
	}
}
