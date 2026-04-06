using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("F�sicas de Salto")]
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundDistance = 0.4f; 
    [SerializeField] private LayerMask groundMask; 
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Ajustes de Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Ajustes de C�mara")]
    [SerializeField] private Transform cameraTransform; // Arrastr� la Main Camera ac�
    [SerializeField] private float mouseSensitivity = 25f;
    [SerializeField] private float upperLookLimit = 80f;
    [SerializeField] private float lowerLookLimit = -80f;

    private CharacterController controller;
    private PlayerInputs inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputs();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Start()
    {
        // Oculta el cursor y lo bloquea en el centro
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        HandleGravity();
    }

    private void HandleMovement()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        // Calculamos la direcci�n relativa a hacia donde mira el cuerpo
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Rotaci�n Vertical (C�mara)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, lowerLookLimit, upperLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotaci�n Horizontal (Cuerpo del jugador)
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleGravity()
    {
        // 1. Detecci�n de suelo mejorada (Esfera en los pies)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. L�gica de Salto
        if (inputActions.Player.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 3. Efecto Caida
        float currentGravity = (velocity.y < 0) ? gravity * fallMultiplier : gravity;

        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}