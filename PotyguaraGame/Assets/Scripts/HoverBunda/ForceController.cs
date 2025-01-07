using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ForceController : MonoBehaviour
{
    // Velocidade de rota��o

    public InputAction headRotationInput; 
    public float rotationSpeed = 100f;
    // O eixo de rota��o (em torno do eixo Y para rota��o horizontal)
    public float smoothTime = 0.1f;
    float targetRotationL;
    float targetRotationR;
    // Vari�veis para controlar a rota��o
    private float currentRotation = 0f;

    bool isLeft = false;
    bool isRight = false;

    public InputActionAsset input;

    // Start is called before the first frame update
    void Start()
    {
        input.FindActionMap("XRI Head").FindAction("Rotation");
    }

    // Update is called once per frame
    /*void Update()
    {
        if (isLeft)
        {
            targetRotationR = (transform.eulerAngles.y - 30);
            float smoothRotationLeft = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotationL, ref currentRotation, smoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothRotationLeft, 0f);
        }
        if (isRight)
        {
            float smoothRotationRight = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotationR, ref currentRotation, smoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothRotationRight, 0f);
        }

        if (Input.GetKey(KeyCode.I))
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }

        if(Input.GetKey(KeyCode.O))
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
    }*/

    void Update()
    {
        // A rota��o da cabe�a normalmente vem do XRNode.Head
        // Obt�m a rota��o da cabe�a em rela��o ao mundo
        Quaternion headRotation = GetHeadRotation();

        // Exemplo de como usar a rota��o: aplica a rota��o da cabe�a a um objeto
        transform.rotation = headRotation;

        // Para debug, voc� pode imprimir a rota��o
        Debug.Log("Head Rotation: " + headRotation.eulerAngles);
    }

    // Fun��o para obter a rota��o da cabe�a
    Quaternion GetHeadRotation()
    {
        // Aqui voc� acessa o dispositivo de cabe�a atrav�s do XRNode
        UnityEngine.XR.InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

        if (headDevice.isValid)
        {
            // Retorna a rota��o do dispositivo da cabe�a (um Quaternion)
            if (headDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.centerEyeRotation, out Quaternion rotation))
            {
                return rotation;
            }
        }

        // Caso n�o consiga pegar a rota��o, retorne a rota��o padr�o (identidade)
        return Quaternion.identity;
    }
}
