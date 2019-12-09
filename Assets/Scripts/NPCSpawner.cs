using UnityEngine;
using UnityEngine.Networking;

public class NPCSpawner : NetworkBehaviour
{
	public GameObject npcPrefab;
	public int numOfNpcs;

	private void Start() {
		if (isServer) {
			for (int i = 0; i < numOfNpcs; i++) {
				var spawnPosition = new Vector3(Random.Range(-8.0f, 8.0f), 0.0f, Random.Range(-8.0f, 8.0f));
				var spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);
				var npc = (GameObject)Instantiate(npcPrefab, spawnPosition, spawnRotation, this.transform);
				NetworkServer.Spawn(npc);
			}

			GameManager.globalInstance.npcCount += numOfNpcs;
			GameManager.globalInstance.npcSpawningComplete = true;
		}
	}
}