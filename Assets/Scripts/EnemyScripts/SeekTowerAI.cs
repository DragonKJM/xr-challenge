using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTowerAI : MonoBehaviour
{
    [Header("Config")]
    //Fighting
    [SerializeField]
    private float attackPower = 5.0f;
    private float attackDelay = 2.0f;
    private float timeSinceAttack = 0.0f;

    //Movement 
    private Vector3 tempVelocity = new Vector3();
    private Vector3 desiredVelocity = new Vector3();
    private Vector3 steering = new Vector3();
    private float maxSpeed = 1.0f;

    private float weight = 5.0f;

    //Seek Variables
    private float radiusAroundTarget = 1.0f;

    [Header("References")]
    //Instead of just seek tower, this could have a list of gameobjects with tag 'building', and seek the closest of those
    private Transform seekTarget;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        seekTarget = FindObjectOfType<WatchTowerPickupGenerator>().transform; // Won't work properly with multiple towers in a real game
    }

    private void FixedUpdate()
    {
        // Force physics
        Vector3 force = GenerateForce();
        Vector3 acceleration = force / rb.mass;
        tempVelocity += acceleration * Time.fixedDeltaTime;
        tempVelocity = Vector3.ClampMagnitude(tempVelocity, maxSpeed);

        rb.velocity = tempVelocity;

        timeSinceAttack += Time.fixedDeltaTime;
        if ((seekTarget.position - transform.position).magnitude <= radiusAroundTarget && timeSinceAttack > attackDelay)
        {
            Attack(seekTarget.GetComponent<HealthManager>());
            timeSinceAttack = 0.0f;
        }
    }

    private Vector3 GenerateForce()
    {
        //Seek
        desiredVelocity = seekTarget.position - transform.position; // Get pos to move to

        if (desiredVelocity.magnitude > radiusAroundTarget) // AI will get up to specified distance of seek target
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed; // Change into direction and scale

            steering = desiredVelocity - tempVelocity; // Turning forces
        }
        else
        {
            steering = Vector3.zero;
            tempVelocity = Vector3.zero; // Stop the AI
        }

        return (steering * weight);
    }

    private void Attack(HealthManager attackedTarget)
    {
        attackedTarget.TakeDamage(attackPower);
        attackedTarget.Shake(0.5f, 0.1f);
    }
}
