using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementOnTheBoard : MonoBehaviour
{
    private InputAction moveAction;
    public float turnSpeed = 60f;

    private Rigidbody rb;
    void Start()
    {
        moveAction = new InputAction("Move", binding: "<XRController>{LeftHand}/thumbstick");
        moveAction.Enable();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        float turn = input.x * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up, turn);
        Vector3 torque = transform.right * input.x * turnSpeed * Time.fixedDeltaTime;
        rb.AddTorque(torque, ForceMode.Force);
        
    }
}
