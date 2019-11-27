using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public bool gravity = true;
    // Use this for initialization
    void Start()
    {
        Debug.Log(transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().useGravity = gravity;
        }

    }
}
