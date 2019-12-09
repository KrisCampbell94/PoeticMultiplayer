using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundRadius = 1;
    public bool isGrounded {get; private set; }

    // Update is called once per frame
    void Update()
    {
        
        Collider[] groundColliders = Physics.OverlapSphere(groundCheck.position, groundRadius, groundLayer);

        isGrounded = (groundColliders.Length > 0);
    }
}
