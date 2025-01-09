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

    private Transform board;

    private bool gameStarted = false;

    private void OnEnable()
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
    }
    // Start is called before the first frame update
    void Start()
    { 
        board = GameObject.Find("board").transform;
    }

    public void SetBoolean()
    {
        gameStarted = true;
    }

    void Update()
    {
        if (gameStarted)
        {
            board.transform.position = new Vector3(540f, 56.2f, -509f);
            board.transform.rotation = new Quaternion(0, -0.361624479f, 0, 0.932323813f);

            Vector2 leftInput = leftJoystick.ReadValue<Vector2>();
            Vector2 rightInput = rightJoystick.ReadValue<Vector2>();

            Vector3 movement = new Vector3(leftInput.x, 0, leftInput.y);
            transform.Translate(movement * Time.deltaTime, Space.World);

            Vector3 rotation = new Vector3(0, rightInput.x, 0);
            transform.Rotate(rotation*Time.deltaTime, Space.World);
        }
    }
}
