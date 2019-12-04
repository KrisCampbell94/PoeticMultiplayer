using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static NPCController;

public class PlayerAllyController : NetworkBehaviour
{
    public GameObject npcPrefab;

    public GameObject lastAlly;

    public int allyCount { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (this.isLocalPlayer)
        {
            InitAllies();
        }
    }
    
    void InitAllies()
    {
        var ally1 = Instantiate(npcPrefab, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(ally1);
        AddAlly(ally1);

        var ally2 = Instantiate(npcPrefab, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(ally2);
        AddAlly(ally2);
    }

    public void AddAlly(GameObject ally)
    {
        NPCController allyController = ally.GetComponent<NPCController>();

        // If have any ally,
        if (lastAlly != null)
        {
            // Set as follower of last ally
            NPCController lastAllyController = lastAlly.GetComponent<NPCController>();
            lastAllyController.follower = allyController;
            allyController.leader = lastAlly.transform;
        } else
        {
            // Set as follower of self
            allyController.leader = this.transform;
        }

        allyController.state = NPCState.Follow;

        allyController.playerAllyController = this;
        lastAlly = ally;
        allyCount++;
    }

    public void RemoveAlly(GameObject ally)
    {
        NPCController allyController = ally.GetComponent<NPCController>();

        if (allyController.playerAllyController == this) // Ally of this player
        {
            if (ally == lastAlly) // Is last ally
            {
                if (allyCount > 1)
                {
                    // Set leader as last ally instead of self
                    lastAlly = allyController.leader.gameObject;
                } else
                {
                    // Lost all allies, do lose condition
                }
            } else if (allyController.follower != null) // Is not last and has follower
            {
                allyController.follower.leader = allyController.leader; // Set new leader for follower
            }

            allyController.leader = null; // Set no leader for self

            allyController.state = NPCState.Patrol;

            allyController.playerAllyController = null;
            allyCount--;
        }
    }
}
