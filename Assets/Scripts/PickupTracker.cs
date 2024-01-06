using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTracker : MonoBehaviour
{
    [Header("Tracked Info")]
    private int numPickupsCollected;

    [Header("References")]
    [SerializeField]
    private List<Pickup> pickups = new List<Pickup>();
    private ScoreUIHandler scoreScript;
    private Goal goal;

    private void Awake()
    {
        numPickupsCollected = 0;

        scoreScript = FindObjectOfType<ScoreUIHandler>();
        if (scoreScript == null)
        {
            Debug.LogError(this + " couldn't find ScoreUIHandler");
        }

        goal = FindObjectOfType<Goal>();
        if (goal == null)
        {
            Debug.LogError(this + " couldn't find Goal");
        }
    }

    private void Start()
    {
        Invoke("DelayedStart", 0.1f); // Need Pickups to finish initialising before the tracker can start
    }

    private void DelayedStart()
    {
        Pickup[] allPickups = FindObjectsOfType<Pickup>();

        foreach (Pickup pickup in allPickups)
        {
            AddPickup(pickup);
        }
    }

    public void AddPickup(Pickup pickup) // Add to list and subscribe to event
    {
        //pickup.Init();

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

    public void CollectPickup(Pickup pickup) // Used for when player picks up Pickup, incase there's any other reason to RemovePickup without incrementing collected
    {
        RemovePickup(pickup);

        numPickupsCollected++;
        if(numPickupsCollected >= 5 && !goal.IsActive)
        {
            goal.Activate();
        }
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

        CollectPickup(pickup);
        Debug.Log("Picked up Pickup!");
    }
}
