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
    public bool IsPlaced = false; // Should use a setter instead of public variable
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
            }
        }
    }

    private void Update()
    {
        progressBar.canvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        if (!IsPlaced)
            return;

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
                pickup.transform.position = transform.position;

                Rigidbody pickupRigidbody = pickup.AddComponent<Rigidbody>(); // Allows below physics simulation

                float jumpHeight = 3.0f;
                pickupRigidbody.velocity = new Vector3(0.0f, Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics.gravity.y)), 0.0f) + transform.forward; // Formula finds initial velocity (sqrt) for up and down (2) of jumpheight, and negates gravity

                tracker.AddPickup(pickup.GetComponent<Pickup>()); // Log pickup and subscribe events

                StartCoroutine(RemovePickupRigidbody(pickupRigidbody)); // Remove rigidbody after reaching specified Y pos, so it doesn't fall through the floor

                return; // Return after finding first available pickup
            }
        }
    }

    IEnumerator RemovePickupRigidbody(Rigidbody rigidbodyToRemove)
    {
        float desiredYPos = 0.0f;

        while (rigidbodyToRemove != null && rigidbodyToRemove.gameObject.transform.position.y >= desiredYPos)
        {
            yield return null; // Wait for the next frame
        }

        if (rigidbodyToRemove != null)
        {
            Destroy(rigidbodyToRemove);
        }
    }

}
