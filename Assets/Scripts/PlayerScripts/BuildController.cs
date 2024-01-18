using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildController : MonoBehaviour
{
    [Header("Config")]
    private Builds selectedBuild;

    [SerializeField]
    private float placementRadius = 2.0f;
    private Vector3 placementDirection;
    public Vector3 PlacementDirection => placementDirection; //used in PlayerController

    private bool watchTowerPlaced = false; // Wouldn't be used in a real game, this is to limit user to a single watchTower

    [Header("References")]
    [SerializeField]
    private GameObject watchTower;

    private GameObject previewObject;
    private GameObject lastPlaced;

    [Header("Data")]
    private Dictionary<Builds, GameObject> buildObjects = new Dictionary<Builds, GameObject>();

    private enum Builds
    {
        NULL,
        WATCHTOWER,
        // Other builds
    }

    private void Awake()
    {
        selectedBuild = Builds.NULL;
    }

    void Start()
    {
        buildObjects.Add(Builds.NULL, null);
        buildObjects.Add(Builds.WATCHTOWER, watchTower);
    }

    void Update()
    {
        if (selectedBuild != Builds.NULL)
        {
            UpdatePreviewPosition();
        }
    }

    public void OnBuildSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        InputControl control = context.control;

        if (control.name == "1" || control.name == "dpad/up")
        {
            if (selectedBuild == Builds.WATCHTOWER || watchTowerPlaced == true) // Already selected '1' or placed it, so deselect
            {
                selectedBuild = Builds.NULL;
                DestroyPreviewObject();
            }
            else
            {
                selectedBuild = Builds.WATCHTOWER;
                CreatePreviewObject(buildObjects[selectedBuild]);
            }
        }
        else if (control.name == "2" || control.name == "dpad/right")
        {
            // Other builds
        }
    }

    public void OnBuildPlacement(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        else
            PlaceBuildObject(buildObjects[selectedBuild]);
    }

    void PlaceBuildObject(GameObject buildObject)
    {
        if (buildObject != null)
        {
            lastPlaced = Instantiate(buildObject, previewObject.transform.position, previewObject.transform.rotation); // Place where preview is showing

            if (selectedBuild == Builds.WATCHTOWER)
            {
                watchTowerPlaced = true;

                WatchTowerPickupGenerator pickupGenerator = lastPlaced.GetComponent<WatchTowerPickupGenerator>(); // Tell watchTower it's been placed
                if (pickupGenerator != null)
                {
                    pickupGenerator.IsPlaced = true;
                }
                else
                {
                    Debug.LogError("watchTowerPickupGenerator not found by BuildController");
                }
            }

            DestroyPreviewObject();
            selectedBuild = Builds.NULL; // Deselect
        }
    }

    void CreatePreviewObject(GameObject buildPrefab)
    {
        if (buildPrefab == null)
            return;

        float heightAdjustment = 0.0f;

        if (selectedBuild == Builds.WATCHTOWER) // Easy way of doing this. Better solution would be to store this information somewhere, or make models a uniform height
            heightAdjustment = 0.8f;

        Vector3 previewPosition = new Vector3(transform.position.x, heightAdjustment, transform.position.z) + placementDirection * placementRadius;
        Vector3 previewRotation = -placementDirection; // Face opposite direction - towards player

        previewObject = Instantiate(buildPrefab, previewPosition, Quaternion.LookRotation(previewRotation));
    }

    public void UpdatePlacementDirection(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.control.name == "position") // If user is using mouse
        {
            Vector2 mousePositionScreen = context.ReadValue<Vector2>();

            // Makes ray at camera point equivalent to mouse position
            Ray ray = Camera.main.ScreenPointToRay(mousePositionScreen);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Direction from transform to hit
                placementDirection = (hit.point - transform.position).normalized;
                placementDirection = new Vector3(placementDirection.x, 0.0f, placementDirection.z); // Negate Y difference
            }
            else
            {
                // Default resort if ray hits nothing
                placementDirection = transform.position + transform.forward * 2.0f;
            }
        }
        else if (context.control.name == "rightStick") // GamePad
        {
            placementDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y).normalized; // Assuming this just gives values between 0 and 1
        }
        else
        {
            Debug.LogError("Directional input not recognised: " + context.control.name);
        }
    }

    void UpdatePreviewPosition()
    {
        float heightAdjustment = 0.0f;

        if (selectedBuild == Builds.WATCHTOWER)
            heightAdjustment = 0.8f;

        Vector3 newPosition = new Vector3(transform.position.x, heightAdjustment, transform.position.z) + placementDirection * placementRadius;
        Vector3 newRotation = -placementDirection;

        if (previewObject != null)
        {
            previewObject.transform.position = newPosition;
            previewObject.transform.rotation = Quaternion.LookRotation(newRotation);
        }
    }

    void DestroyPreviewObject()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }
}
