﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;// <====

public class PlayerController : NetworkBehaviour
{
    public float jumpForce = 10;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        if (this.isLocalPlayer)
        {
            rb = GetComponent<Rigidbody>();
        }
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
        Cursor.lockState = CursorLockMode.Locked;
        float vMove = Input.GetAxis("Vertical") * Time.deltaTime * 15;
        float hMove = Input.GetAxis("Horizontal") * Time.deltaTime * 15;
        // float vRotate = (Input.GetAxis("Mouse Y") * Time.deltaTime * 30) *-1;
        float hRotate = Input.GetAxis("Mouse X") * Time.deltaTime * 30;

        transform.Translate(hMove, 0, vMove);
        transform.Rotate(0, hRotate, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForce, rb.velocity.z);
        }
    }

    public override void OnStartLocalPlayer()
    {
        transform.Find("Body").GetComponent<MeshRenderer>().material.color = Color.blue;
        transform.Find("Gun").GetComponent<MeshRenderer>().material.color = Color.red;
    }

}
