using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAI : MonoBehaviour
{
    [Header("Config")]
    //Movement 
    private Vector3 tempVelocity = new Vector3();
    private Vector3 desiredVelocity = new Vector3();
    private Vector3 steering = new Vector3();
    private float maxSpeed = 1.0f;

    private float weight = 5.0f;

    //Wander variables
    public float WanderRadius = 2;
    public float WanderOffset = 2;
    public float AngleDisplacement = 20;

    private Vector3 circlePosition = new Vector3();
    private Vector3 pointOnCircle = new Vector3();
    private float angle = 0.0f;

    [Header("References")]
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(ExecuteInBursts());
    }

    private IEnumerator ExecuteInBursts()
    {
        while (true)
        {
            float duration = Random.Range(1.0f, 3.0f);
            
            float startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                WanderUpdate(); // Call WanderUpdate as a fixedUpdate
                yield return new WaitForFixedUpdate();
            }

            rb.velocity = Vector3.zero; // Stop and wait
            yield return new WaitForSeconds(Random.Range(0.1f, 2.0f));
        }
    }

    private void WanderUpdate()
    {
        // Force physics
        Vector3 force = GenerateForce();
        Vector3 acceleration = force / rb.mass;
        tempVelocity += acceleration * Time.fixedDeltaTime;
        tempVelocity = Vector3.ClampMagnitude(tempVelocity, maxSpeed);

        rb.velocity = tempVelocity;
    }

    private Vector3 GenerateForce()
    {
        //Calculate wander position
        float random = Random.Range(-1.0f, 1.0f);

        if (random >= 0)
            angle -= AngleDisplacement; // Move left or right
        else
            angle += AngleDisplacement;

        float rad = angle * Mathf.Deg2Rad;

        pointOnCircle.x = Mathf.Cos(rad); //Calculate a 2D point on a circle
        pointOnCircle.y = 0.0f;
        pointOnCircle.z = Mathf.Sin(rad);

        pointOnCircle = pointOnCircle * WanderRadius; // Move point into radius

        circlePosition = transform.position;
        circlePosition +=  tempVelocity.normalized * WanderOffset; // Place circle in front of AI

        pointOnCircle += circlePosition; //Move point onto the projected circle

        //Seek
        desiredVelocity = pointOnCircle - transform.position; // Get pos to move to

        desiredVelocity = desiredVelocity.normalized * maxSpeed; // Change into direction and scale

        steering = desiredVelocity - tempVelocity; // Turning forces

        return (steering * weight);
    }
}
