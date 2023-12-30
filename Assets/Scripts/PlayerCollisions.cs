using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [Header("References")]
    private PickupTracker pickupTracker;

    private void Awake()
    {
        pickupTracker = FindObjectOfType<PickupTracker>();

        if (pickupTracker == null)
        {
            Debug.LogError(this + " couldn't find PickupTracker");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Pickup")) // Would normally use a switch statement here, but using CompareTag seems more worthwhile
        {
            if (other.transform.TryGetComponent(out Pickup pickupScript))
            {
                pickupScript.GetPickedUp();
            }
            else
            {
                Debug.LogError("Pickup script not found on the collided GameObject.");
            }
        }

        else if (other.transform.CompareTag("Finish"))
        {
            Debug.Log("PLAYER REACHED GOAL");
        }

        else
        {
            Debug.LogError("Player Trigger not recognised");
        }
    }
}
