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

    private float gunRotateSpeed = 30;

    private float timeAtLastShot = 0;
    
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
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        if (this.isLocalPlayer)
        {
            bullet.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        bullet.GetComponent<Rigidbody>().velocity = gun.transform.up * bulletSpeed;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, bulletDespawnTimer);
    }
}
