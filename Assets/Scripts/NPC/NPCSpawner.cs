using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NPCSpawner : NetworkBehaviour
{

    public GameObject npcPrefab;
    public int numOfNpcs;
    private GameObject[] wallObjects;

    private void Start()
    {
        if (isServer)
        {
            GameManager.globalInstance.npcCount += numOfNpcs;
            GameManager.globalInstance.npcSpawningComplete = true;
        }
    }

    public override void OnStartServer()
    {
        wallObjects = GameObject.FindGameObjectsWithTag("Wall");

        for (int i = 0; i < numOfNpcs; i++)
        {
            bool spawnable = true;
            var spawnPosition = new Vector3(Random.Range(-8.0f, 8.0f), 0.0f, Random.Range(-8.0f, 8.0f));
            // This checks to see if the spawn position is located within a wall
            foreach (GameObject wall in wallObjects)
            {
                Vector3 size = wall.GetComponent<MeshRenderer>().bounds.size;
                float distance = Vector3.Distance(spawnPosition, wall.transform.position);
                // The 4 additive is a double check
                //      Without it, some spawns will appear inside the wall
                if (distance <= (size.x + 4) && distance <= (size.z + 4))
                {
                    // Debug.Log(wall.name + " Contains Enemy " + i);
                    spawnable = false;
                    break;
                }
            }
            // If the position is outside the walls, create the spawn
            if (spawnable)
            {
                var spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);
                var enemy = (GameObject)Instantiate(npcPrefab, spawnPosition, spawnRotation, this.transform);
                NetworkServer.Spawn(enemy);
            }
            // Try again by deducting the counter by 1
            else
            {
                i -= 1;
                continue;
            }
        }
    }
}