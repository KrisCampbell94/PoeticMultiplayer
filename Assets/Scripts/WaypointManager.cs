using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
	private List<Transform> waypoints;

    // Start is called before the first frame update
    void Start()
    {
		waypoints = new List<Transform>();

		foreach (Transform child in this.transform) {
			waypoints.Add(child);
		}
    }

	public Transform GetWayPoint(int index) {
		return waypoints[index];
	}

	public Transform GetRandomWayPoint() {
		int rand = Random.Range(0, waypoints.Count);
		return waypoints[rand];
	}
}
