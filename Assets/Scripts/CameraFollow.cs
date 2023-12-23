using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Config")]
    public Vector3 offset;

    [Header("References")]
    [SerializeField]
    private Transform target;

    private void Awake()
    {
        if (target == null)
        {
            if (target != GameObject.FindGameObjectWithTag("Player").transform)
            {
                Debug.LogError("Camera could not find Player!");
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {     
            transform.position = target.position + offset;
        }
    }
}
