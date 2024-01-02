using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float distanceThreshold = 5f;
    [SerializeField]
    private float minAlpha = 0.3f;
    [SerializeField]
    private float maxAlpha = 1f;

    [Header("References")]
    private Renderer wallRenderer;
    private Transform mainCamera;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main.transform;

        if (wallRenderer == null || mainCamera == null)
        {
            Debug.LogError("WallTransparency script requires a Renderer component and a Main Camera");
            enabled = false;
        }
    }

    void Update()
    {
        Vector3 toCameraDirection = (mainCamera.position - transform.position).normalized;

        if (Vector3.Dot(toCameraDirection, transform.forward) <= 0.75) // Relative direction between forward and toCamera, 1.0 is parallel
        {
            float distanceToCamera = Vector3.Distance(mainCamera.position, transform.position);

            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.InverseLerp(distanceThreshold, distanceThreshold + 1.0f, distanceToCamera)); // Inverse lerp to get a lerp amount for alpha

            Color currentColor = wallRenderer.material.color;
            wallRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha); // Just old colour with new alpha
        }
        else
        {
            Color currentColor = wallRenderer.material.color;
            wallRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f); // Reset alpha
        }
    }
}
