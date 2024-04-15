using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ForceController : MonoBehaviour
{
    public GameObject xrRig;
    private Rigidbody rig;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
     rig = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Fixedpdate()
    {
        float xInput = Input.GetAxis("Horizontal");
        rig.AddForce(Vector3.right * xInput * force);
    }
}
