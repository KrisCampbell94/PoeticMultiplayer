using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAllyController : NetworkBehaviour
{
    public GameObject npcPrefab;
    public Transform allyFollowPos;

    public GameObject lastAlly;

    // Start is called before the first frame update
    void Start()
    {
        if (this.isLocalPlayer)
        {
            InitAllies();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isLocalPlayer)
        {

        }
    }

    [Command]
    void InitAllies()
    {
        var ally1 = Instantiate(npcPrefab, allyFollowPos.position, this.transform.rotation);
        NPCController ally1Controller = ally1.GetComponent<NPCController>();
        ally1Controller.SetLeader(this.transform);
        NetworkServer.Spawn(ally1);
    }
}
