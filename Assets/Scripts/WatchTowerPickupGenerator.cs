using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WatchTowerPickupGenerator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float timeToGenerate;

    [Header("References")]
    [SerializeField]
    private List<GameObject> pickups = new List<GameObject>();
    private PickupTracker tracker;
    private Image progressBar;

    [Header("Spawn Tracking")]
    private float timer = 0.0f;

    private void Start()
    {
        tracker = FindObjectOfType<PickupTracker>();
        pickups = GameObject.FindGameObjectsWithTag("Pickup").ToList();

        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas != null)
        {
            Image imageInCanvas = canvas.GetComponentInChildren<Image>();

            if (imageInCanvas != null)
            {
                progressBar = imageInCanvas.transform.Find("ProgressBar").GetComponent<Image>(); // For some reason, using GetComponentInChildren again here returns the imageInCanvas again

                if (progressBar != null)
                {
                    Debug.Log("got Image " + progressBar.name);
                }
            }
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        progressBar.fillAmount = timer / timeToGenerate;

        if (timer >= timeToGenerate)
        {
            timer = 0.0f;

            SpawnPickup();
        }
    }

    void SpawnPickup()
    {
        foreach (GameObject pickup in pickups)
        {
            Pickup pickupScript = pickup.GetComponent<Pickup>();

            if (pickupScript.IsCollected)
            {
                pickupScript.Init();
                pickup.transform.position = transform.position + new Vector3(2.0f, 0.0f, 0.0f);

                tracker.AddPickup(pickup.GetComponent<Pickup>());

                return; // Return after finding first available pickup
            }
        }
    }

}
