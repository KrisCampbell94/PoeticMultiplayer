using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
	public PlayerAllyController playerAllyController;

	void OnCollisionEnter(Collision collision) {
        if (isServer)
        {
            var collisionObj = collision.gameObject;

            // Is not wall
            if (collisionObj.layer != LayerMask.NameToLayer("Wall"))
            {
                PlayerController playerController = collisionObj.GetComponent<PlayerController>();
                // Is not player OR is player but not self
                if (playerController == null || !playerController.isLocalPlayer)
                {
                    // If hit npc, convert it using ally controller
                    if (collisionObj.tag == "NPC")
                    {
                        playerAllyController.CmdOnHitNPC(collisionObj);
                    }

                    // Destroy bullet
                    Destroy(this.gameObject);
                }
            }
        }
	}
}
