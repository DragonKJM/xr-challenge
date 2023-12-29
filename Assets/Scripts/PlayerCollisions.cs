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
        if (other.transform.CompareTag("Pickup"))
        {
            if (other.transform.TryGetComponent(out Pickup pickupScript))
            {
                pickupScript.GetPickedUp();
                Debug.Log("Picked up Pickup!");
                pickupTracker.RemovePickup(pickupScript);
            }
            else
            {
                Debug.LogError("Pickup script not found on the collided GameObject.");
            }
        }
    }
}
