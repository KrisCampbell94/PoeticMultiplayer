using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer != 8)
        {
            if (collision.gameObject.tag != "Player" ||
            !collision.gameObject.GetComponent<PlayerController>().isLocalPlayer)
            {
                var hit = collision.gameObject;
                var healthScript = hit.GetComponent<Health>();
                if (healthScript != null)
                {
                    healthScript.TakeDamage(10);
                    Debug.Log("I hit the player " + hit.GetInstanceID() + ": It's health is: " + healthScript.currentHealth + ". Bye from me!!!");
                }
                Destroy(this.gameObject);
            }
        }
        
	}
}
