using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ForceController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions; 
    private InputAction leftJoystick;
    private InputAction rightJoystick;

    public float speed = 10f;
    public float turnSpeed = 5f;
    public float gravityMultiplier = 1.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private bool gameStarted = false;

   /* private void OnEnable()
    {
        var actionMap = inputActions.FindActionMap("Joysticks");
        leftJoystick = actionMap.FindAction("Primary2DAxis");
        rightJoystick = actionMap.FindAction("Secondary2DAxis");

        leftJoystick.Enable();
        rightJoystick.Enable();
    }

    private void OnDisable()
    {
        leftJoystick.Disable();
        leftJoystick.Disable();
    }*/

    public void SetBoolean()
    {
        gameStarted = true;
    }

    void Update()
    {
        if (gameStarted)
        {
            transform.position = new Vector3(540f, 56.2f, -509f);
            transform.rotation = new Quaternion(0, -0.361624479f, 0, 0.932323813f);

            HandleMovement();
        }
    }

    void HandleMovementWithOculus()
    {
        Vector2 leftInput = leftJoystick.ReadValue<Vector2>();
        Vector2 rightInput = rightJoystick.ReadValue<Vector2>();

        Vector3 movement = new Vector3(leftInput.x, 0, leftInput.y);
        transform.Translate(movement * Time.deltaTime, Space.World);

        Vector3 rotation = new Vector3(0, rightInput.x, 0);
        transform.Rotate(rotation * Time.deltaTime, Space.World);

        // Simulação de gravidade extra para evitar deslizar no ar
        rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

    void HandleMovement()
    {
        // Movimento para frente baseado na inclinação
        //Vector3 movement = transform.forward* speed;
        //transform.Translate(movement * Time.deltaTime, Space.World);
            

        // Simulação de gravidade extra para evitar deslizar no ar
        //rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

}
