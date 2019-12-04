using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NPCController : NetworkBehaviour
{
    public enum NPCState { Patrol, Follow }

    public PlayerAllyController playerAllyController;
    public Transform leader;
    public NPCController follower;
    public NPCState state = NPCState.Patrol;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            UpdateNPC();
        }
    }

    private void UpdateNPC()
    {
        switch (this.state)
        {
            case NPCState.Patrol:
                Patrol();
                break;
            case NPCState.Follow:
                Follow();
                break;
        }
    }

    private void Patrol()
    {

    }

    private void Follow()
    {
        navMeshAgent.destination = leader.position;
    }
}
