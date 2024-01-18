using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool IsActive { get; private set; }

    void Start()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        IsActive = true;
    }
}
