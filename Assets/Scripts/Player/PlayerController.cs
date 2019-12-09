using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;// <====

public class PlayerController : NetworkBehaviour {
    private float jumpForce = 6;

    private Rigidbody rBody;
    private GroundChecker groundChecker;

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();
        groundChecker = GetComponent<GroundChecker>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isLocalPlayer)
        {
            UpdatePlayer();
        }
    }

    void UpdatePlayer()
    {
        // Mouse Lock States
        if (Input.GetButton("Cancel") && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetButton("Fire1") && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        float vMove = Input.GetAxis("Vertical") * Time.deltaTime * 15;
        float hMove = Input.GetAxis("Horizontal") * Time.deltaTime * 15;
        // float vRotate = (Input.GetAxis("Mouse Y") * Time.deltaTime * 30) *-1;
        float hRotate = Input.GetAxis("Mouse X") * Time.deltaTime * 30;

        transform.Translate(hMove, 0, vMove);
        transform.Rotate(0, hRotate, 0);

        if (Input.GetButton("Jump") && groundChecker.isGrounded)
        {
            rBody.velocity = new Vector3(rBody.velocity.x, rBody.velocity.y + jumpForce, rBody.velocity.z);
        }

		// Stabilize
		rBody.angularVelocity = new Vector3(0,0,0);
    }
}
