using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;// <====

public class PlayerController : NetworkBehaviour {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
    public GameObject gun;
	public float bulletSpeed = 30;
    public float bulletTimer = 10;
    public float jumpForce = 10;

    private Rigidbody rb;

    private Camera camera;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        CameraUpdate();
        if (isClient)
        {
            CameraPosition();
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (!this.isLocalPlayer)
			return;


        Cursor.lockState = CursorLockMode.Locked;
        float vMove = Input.GetAxis ("Vertical") * Time.deltaTime * 15;
        float hMove = Input.GetAxis("Horizontal") * Time.deltaTime * 15;
        float vRotate = (Input.GetAxis("Mouse Y") * Time.deltaTime * 30) *-1;
        float hRotate = Input.GetAxis("Mouse X") * Time.deltaTime * 30;

		transform.Translate (hMove, 0, vMove);
		transform.Rotate (0, hRotate, 0);

        gun.transform.Rotate(vRotate, 0, 0);

		//if (Input.GetAxis ("Fire1")>0) {
		if (Input.GetKey(KeyCode.Mouse0)) {
			//Fire ();
			CmdFire (); //networked fire
		}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y + jumpForce, rb.velocity.z);
        }

    }
	[Command]
	void CmdFire(){
		var bullet=Instantiate(bulletPrefab, bulletSpawn.position, this.transform.localRotation);	
		bullet.GetComponent<Rigidbody> ().velocity = gun.transform.up * bulletSpeed ;
		NetworkServer.Spawn (bullet);
		Destroy (bullet, bulletTimer);
	}

	public override void OnStartLocalPlayer(){
		this.GetComponent<MeshRenderer> ().material.color = Color.blue;
	}

    void CameraUpdate()
    {
        if (this.isLocalPlayer)
        {
            GameObject camera_main = GameObject.FindGameObjectWithTag("MainCamera");

            camera = (Camera)Instantiate(camera_main.GetComponent<Camera>(), transform.position, Quaternion.identity);

            //camera_main.SetActive(false);
            Destroy(camera_main);
            camera.enabled = true;
            camera.transform.parent = transform;

        }
    }

    void CameraPosition()
    {
        camera.transform.localPosition = new Vector3(1.41f, 1, -2.03f);
        Debug.Log(Vector3.Distance(transform.position, camera.transform.position));
    }

}
