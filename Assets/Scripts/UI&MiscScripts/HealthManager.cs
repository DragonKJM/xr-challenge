using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float health = 100.0f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (CompareTag("Enemy"))
                FindObjectOfType<ScoreUIHandler>().AddScore(50);
            else if (CompareTag("Building"))
            {
                //end game and stuff;
            }
            Destroy(gameObject);
        }
    }

    public void Shake(float duration, float intensity)
    {
        StartCoroutine(ShakeCoroutine(duration, intensity));
    }

    private IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        Vector3 originalPosition = transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = originalPosition.x + Random.Range(-intensity, intensity); // Get random position in spawner on a plane
            float z = originalPosition.z + Random.Range(-intensity, intensity);

            transform.position = new Vector3(x, originalPosition.y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}
