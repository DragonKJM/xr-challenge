using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTracker : MonoBehaviour
{
    [Header("References")]
    private List<Pickup> pickups = new List<Pickup>();
    private ScoreUIHandler scoreScript;

    private void Awake()
    {
        scoreScript = FindObjectOfType<ScoreUIHandler>();

        if (scoreScript == null)
        {
            Debug.LogError(this + " couldn't find ScoreUIHandler");
        }
    }

    private void Start()
    {
        Invoke("DelayedStart", 0.1f); //Need Pickups to finish initialising before the tracker can start
    }

    private void DelayedStart()
    {
        Pickup[] allPickups = FindObjectsOfType<Pickup>();

        foreach (Pickup pickup in allPickups)
        {
            AddPickup(pickup);
        }
    }

    public void AddPickup(Pickup pickup)
    {
        pickups.Add(pickup);
        pickup.OnPickUp += HandlePickupEvent;
        Debug.Log("Added " + pickup.name + " to tracker");
    }

    public void RemovePickup(Pickup pickup)
    {
        pickups.Remove(pickup);
        pickup.OnPickUp -= HandlePickupEvent;
        Debug.Log("Removed " + pickup.name +" from tracker");
    }

    private void HandlePickupEvent(Pickup pickup)
    {
        Debug.Log("Handling pickup event for: " + pickup.name);

        if (scoreScript != null)
        {
            scoreScript.UpdateScore(pickup.ScoreValue);
        }
        else
        {
            Debug.LogError("scoreScript reference is null");
        }
    }
}
