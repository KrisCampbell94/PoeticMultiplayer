using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NPCController : NetworkBehaviour
{
	public enum NPCState { Patrol, Follow }

	public NPCState state = NPCState.Patrol;

	public PlayerAllyController playerAllyController;
	public Transform leader;
	public NPCController follower;

	private Rigidbody rBody;
	private NavMeshAgent navMeshAgent;

	private Transform patrolTarget;

	// Start is called before the first frame update
	void Start() {
		rBody = GetComponent<Rigidbody>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update() {
		if (isServer) {
			UpdateNPC();
		}
	}

	private void UpdateNPC() {
		switch (this.state) {
			case NPCState.Patrol:
				Patrol();
				break;
			case NPCState.Follow:
				Follow();
				break;
		}

		// Stabilize
		rBody.angularVelocity = new Vector3(0, 0, 0);
	}

	private void Patrol() {
		// https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
		if (!navMeshAgent.pathPending) {
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
				if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
					// If reached destination, set new target
					patrolTarget = WaypointManager.globalInstance.GetRandomWayPoint();
					navMeshAgent.destination = patrolTarget.position;
				}
			}
		}
	}

	private void Follow() {
		navMeshAgent.destination = leader.position;
	}
}
