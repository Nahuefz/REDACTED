using System;
using System.Collections;
using Enemy.AngryEnemy;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("<color=yellow>Movement Variables</color>")]
    public bool canMove = true;
    public float walkSpeed = 5f;
    public float mouseSensitivity = 0.1f;
    public Transform cameraTransform;
    public float jumpHeight = 3f;
    public float airControl = 0.2f;
    [Space(3)]
    [Header("ground raycast")]
    public float groundCheckDistance = 0.5f;
    public  LayerMask groundMask;
    [Header("Juice")]
    public float coyoteTime = 0.15f;
    private float _coyoteTimeCounter;
    [Header("Respawn Settings")]
    public Transform spawnPoint;
    
    private Rigidbody _rb;
    public PlayerInputs _controls;
    private Vector2 _moveInputs;
    private Vector2 _lookInputs;
    private float _xRotation = 0f;
    private bool shouldJump;
    private bool _isRespawning = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _controls = new PlayerInputs();
        
        Cursor.lockState = CursorLockMode.Locked;
        
        if(cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    private void OnEnable() => _controls.Player.Enable();
    private void OnDisable() => _controls.Player.Disable();

    private void Update()
    {
        _moveInputs = _controls.Player.Move.ReadValue<Vector2>();
        _lookInputs = _controls.Player.Look.ReadValue<Vector2>();

        // Bloqueamos el input si no puede moverse o está en diálogo
        if (!canMove) return;
        if (DialogueManager.Instance != null && DialogueManager.Instance.EstaHablando()) return; 

        if (IsGrounded())
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (Time.timeScale != 0f) HandleRotation();

        if (_controls.Player.Jump.WasPerformedThisFrame() && _coyoteTimeCounter > 0f)
        {
            //Debug.Log("Grounded");
            shouldJump = true;
            _coyoteTimeCounter = 0f;
        }
    }

    private void HandleRotation()
    {
        float mouseX = _lookInputs.x * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        
        float mouseY = _lookInputs.y * mouseSensitivity;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); 
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        if (DialogueManager.Instance != null && DialogueManager.Instance.EstaHablando()) return;

        Vector3 moveDir = transform.forward * _moveInputs.y + transform.right * _moveInputs.x;
        Vector3 targetVelocity = moveDir * walkSpeed;

        Vector3 currentVelocity = _rb.linearVelocity;
        Vector3 velocityChange = new Vector3(targetVelocity.x - currentVelocity.x, 0, targetVelocity.z - currentVelocity.z);

        if (!IsGrounded())
        {
            if (_moveInputs.magnitude < 0.1f)
            {
                velocityChange = Vector3.zero;
            }
            else
            {
                velocityChange *= airControl;
            }
        }

        _rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (shouldJump) Jump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pincho"))
        {
            Debug.Log("Jugador eliminado por pinchos. Reapareciendo...");
            Respawn();
        }

        if (other.CompareTag("Placa"))
        {
            Respawn();
        }

        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"Colision con enemigo {other.gameObject.name}");
            
            // Buscamos el componente en el objeto o sus padres (por si el collider está en un hijo)
            var angryEnemy = other.GetComponent<AngryEnemyBehaviour>();
            if (angryEnemy == null) angryEnemy = other.GetComponentInParent<AngryEnemyBehaviour>();

            if (angryEnemy != null)
            {
                angryEnemy.ClearTarget();
            }
            
            Respawn();
        }
    }


    void Jump()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        shouldJump = false;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }
    
    public void LookAtFront()
    {
        _xRotation = 0f;

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public float GetXRotation()
    {
        return _xRotation;
    }

    public void SetXRotation(float xRotation)
    {
        _xRotation = xRotation;
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }

    public void Respawn()
    {
        if (_isRespawning) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        _isRespawning = true;
        canMove = false;
        
        // Detener físicamente al jugador
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;

        TransitionFade fade = UnityEngine.Object.FindFirstObjectByType<TransitionFade>();

        if (fade != null)
        {
            yield return StartCoroutine(fade.FadeToBlack());
        }

        // Opción recomendada: Recargar la escena para resetear todo el nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        /* 
        // Alternativa sin recargar escena (si prefieres solo mover al jugador):
        // transform.position = spawnPoint.position;
        // _rb.position = spawnPoint.position;
        // _rb.isKinematic = false;
        // if (fade != null) yield return StartCoroutine(fade.Fade(1f, 0f));
        // canMove = true;
        // _isRespawning = false;
        */
    }
}