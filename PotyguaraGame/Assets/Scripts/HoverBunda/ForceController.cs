using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ForceController : MonoBehaviour
{
    // Velocidade de rotação

    public InputAction headRotationInput; 
    public float rotationSpeed = 100f;
    // O eixo de rotação (em torno do eixo Y para rotação horizontal)
    public float smoothTime = 0.1f;
    float targetRotationL;
    float targetRotationR;
    // Variáveis para controlar a rotação
    private float currentRotation = 0f;
    private Transform board;

    bool isLeft = false;
    bool isRight = false;

    private bool gameStarted = false;

    public InputActionAsset input;

    // Start is called before the first frame update
    void Start()
    {
        //input.FindActionMap("XRI Head").FindAction("Rotation");
        board = GameObject.Find("board").transform;
    }

    public void SetBoolean()
    {
        gameStarted = true;
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
        if (gameStarted)
        {
            board.transform.position = new Vector3(540f, 56.2f, -509f);
            board.transform.rotation = new Quaternion(0, -0.361624479f, 0, 0.932323813f);
            GetComponent<ConstantForce>().relativeForce = new Vector3(5f, 0f, 5f);
        }
        // A rotação da cabeça normalmente vem do XRNode.Head
        // Obtém a rotação da cabeça em relação ao mundo
        //Quaternion headRotation = GetHeadRotation();

        // Exemplo de como usar a rotação: aplica a rotação da cabeça a um objeto
        //transform.rotation = headRotation;

        // Para debug, você pode imprimir a rotação
        //Debug.Log("Head Rotation: " + headRotation.eulerAngles);
    }

    // Função para obter a rotação da cabeça
    Quaternion GetHeadRotation()
    {
        // Aqui você acessa o dispositivo de cabeça através do XRNode
        UnityEngine.XR.InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

        if (headDevice.isValid)
        {
            // Retorna a rotação do dispositivo da cabeça (um Quaternion)
            if (headDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.centerEyeRotation, out Quaternion rotation))
            {
                return rotation;
            }
        }

        // Caso não consiga pegar a rotação, retorne a rotação padrão (identidade)
        return Quaternion.identity;
    }
}
