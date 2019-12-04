using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NPCController : NetworkBehaviour
{
    public enum NPCState { Patrol, Follow }

    public Transform leader;
    public NPCController follower;

    private NPCState state;

    // Start is called before the first frame update
    void Start()
    {
        state = NPCState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
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

    }

    //

    public void SetLeader(Transform target)
    {
        state = NPCState.Follow;
        this.leader = target;
    }

    public void ClearLeader()
    {
        state = NPCState.Patrol;
        this.follower.SetLeader(this.leader);
        this.leader = null;
    }
}
