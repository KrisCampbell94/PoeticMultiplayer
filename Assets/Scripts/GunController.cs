using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GameObject gun;

    public float shootDelay = 1;
    public float bulletSpeed = 30;
    public float bulletDespawnTimer = 4;

	private PlayerAllyController playerAllyController;

    private float gunRotateSpeed = 30;

    private float timeAtLastShot = 0;

	private void Start() {
		this.playerAllyController = GetComponent<PlayerAllyController>();
	}

	// Update is called once per frame
	void Update()
    {
        if (this.isLocalPlayer)
        {
            UpdateGun();
        }
    }

    void UpdateGun()
    {
        float vRotate = (Input.GetAxis("Mouse Y") * Time.deltaTime * 30) * -1;
        gun.transform.Rotate(vRotate, 0, 0);

        // if (Input.GetAxis ("Fire1")>0) {
        // if (Input.GetKey(KeyCode.Mouse0)) {
        float timeSinceLastShot = Time.time - timeAtLastShot;
        if (Input.GetButton("Fire1") && timeSinceLastShot >= shootDelay) {
            CmdFire(); //networked fire
            timeAtLastShot = Time.time;
        }
    }

    [Command]
    void CmdFire()
    {
		var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation); // Make new bullet
		bullet.GetComponent<Rigidbody>().velocity = gun.transform.up * bulletSpeed; // Set velocity
		bullet.GetComponent<Bullet>().playerAllyController = this.playerAllyController; // Set self as owner
		NetworkServer.Spawn(bullet); // Spawn on net
		Destroy(bullet, bulletDespawnTimer); // Destroy after a while

		RpcFire(bullet);
	}

	[ClientRpc]
	void RpcFire(GameObject bullet) {
		bullet.GetComponent<MeshRenderer>().material.color = playerAllyController.color; // Set color
	}
}
