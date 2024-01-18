using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

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

    private float attackDistance = 2.0f;
    private float attackDamage = 10.0f;
    private float attackRadius = 1.0f;

    [Header("References")]
    private Rigidbody rb;
    private Camera cam;
    private BuildController buildController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        buildController = GetComponent<BuildController>();
    }

    void Start()
    {
        camForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized; // Using scale to eliminate y axis, normalising to remove potential world-space
        camRight = Vector3.Scale(cam.transform.right, new Vector3(1, 0, 1)).normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = movementDirection * currentPlayerSpeed * Time.fixedDeltaTime;
    }

    public void OnPlayerInputMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            currentPlayerSpeed = 0f; // If button is released, stop moving
            return;
        }

        if (!context.performed)
            return;

        Vector2 newDirection = context.ReadValue<Vector2>(); //Reads direction as X Y coordinates
        movementDirection = camForward * newDirection.y + camRight * newDirection.x; // Y axis is actually our Z axis

        currentPlayerSpeed = basePlayerSpeed; // Set speed on button press, can be multiplied with any modifiers
    }

    public void OnPlayerAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Vector3 attackPos = transform.position + (buildController.PlacementDirection * attackDistance);

        Collider[] hitColliders = Physics.OverlapSphere(attackPos, attackRadius); // Creates sphere to grab colliders in hit location

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform.CompareTag("Enemy")) // If has enemy tag
            {
                HealthManager enemy = hitCollider.GetComponent<HealthManager>();

                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    enemy.Shake(0.1f, 0.05f);
                }
            }
        }

    }
}
