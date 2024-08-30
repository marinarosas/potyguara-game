using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Transform mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(mainCamera.position.x, transform.position.y, mainCamera.position.z);
        transform.eulerAngles = new Vector3(mainCamera.rotation.x, mainCamera.rotation.y, transform.rotation.z);
    }
}
