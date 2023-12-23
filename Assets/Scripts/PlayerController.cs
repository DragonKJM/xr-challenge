using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private Vector3 movementDirection;

    private const float basePlayerSpeed = 100f;
    [SerializeField]
    private float currentPlayerSpeed;

    private Vector3 camForward;
    private Vector3 camRight;

    [Header("References")]
    private Rigidbody rb;
    private Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    void Start()
    {
        camForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized; // using scale to eliminate y axis, normalising to remove potential world-space
        camRight = Vector3.Scale(cam.transform.right, new Vector3(1, 0, 1)).normalized;
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = movementDirection * currentPlayerSpeed * Time.fixedDeltaTime;
    }

    public void OnPlayerInputMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            currentPlayerSpeed = 0f;
            return;
        }

        if (!context.performed)
            return;

        Vector2 newDirection = context.ReadValue<Vector2>();
        movementDirection = camForward * newDirection.y + camRight * newDirection.x; //Y axis is actually the Z axis here

        currentPlayerSpeed = basePlayerSpeed; //will only activate when button is pressed
    }
}
