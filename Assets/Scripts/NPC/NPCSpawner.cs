using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NPCSpawner : NetworkBehaviour
{
	public GameObject npcPrefab;
	public int numOfNpcs;

	private void Start() {
		if (isServer) {
			for (int i = 0; i < numOfNpcs; i++) {
				var spawnPosition = RandomNavmeshLocation(8);
				var spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);
				var npc = (GameObject)Instantiate(npcPrefab, spawnPosition, spawnRotation, this.transform);
				NetworkServer.Spawn(npc);
			}

			GameManager.globalInstance.npcCount += numOfNpcs;
			GameManager.globalInstance.npcSpawningComplete = true;
		}
	}

	// https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
	public Vector3 RandomNavmeshLocation(float radius) {
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas)) {
			finalPosition = hit.position;
		}
		return finalPosition;
	}
}