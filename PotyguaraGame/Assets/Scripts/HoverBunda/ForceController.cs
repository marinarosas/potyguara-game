using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class ForceController : MonoBehaviour
{
    public InputActionReference leftHandPosition;
    public InputActionReference rightHandPosition;
    public GameObject xrRig;
    public float force;

    private Rigidbody rig;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float xInput = Input.GetAxis("Horizontal");
        //rig.AddForce(Vector3.right * xInput * force);
        if (Input.GetKeyDown(KeyCode.A)) // ir para a esquerda
        {
            rig.AddForce(Vector3.up * 1 * force);
        }
        if (Input.GetKeyDown(KeyCode.D)) // ir para a direita
        {
            rig.AddForce(Vector3.up * (-1) * force);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Finish"))
        {
        }
    }
}
