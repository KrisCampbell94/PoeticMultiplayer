using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
{
    public GameObject cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        SetLocalCamera();
    }
    
    void SetLocalCamera()
    {
        if (this.isLocalPlayer)
        {
            GameObject oldCamera = GameObject.FindGameObjectWithTag("MainCamera");
            Camera newCamera = (Camera)Instantiate(oldCamera.GetComponent<Camera>(), transform.position, Quaternion.identity);
            
            Destroy(oldCamera);

            //camera_main.SetActive(false);
            newCamera.enabled = true;
            newCamera.transform.parent = cameraPos.transform;

            //newCamera.transform.localPosition = new Vector3(1.41f, 1, -2.03f);
            // Debug.Log(Vector3.Distance(transform.position, newCamera.transform.position));
        }
    }
}
