using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    [Header("References")]
    WatchTowerPickupGenerator watchTower;

    private void Start()
    {
        watchTower = FindObjectOfType<WatchTowerPickupGenerator>();

        if (watchTower != null && watchTower.IsPlaced)
        {
           gameObject.AddComponent<SeekTowerAI>();
        }
        else
        {
            gameObject.AddComponent<WanderAI>();
        }
    }

    private void FixedUpdate()
    {
        //Could use events here, but they can get confusing, so this is easier for now
        if (watchTower == null || watchTower.IsPlaced == false)
        {
            watchTower = FindObjectOfType<WatchTowerPickupGenerator>();

            if (watchTower != null && watchTower.IsPlaced)
            {
                Destroy(GetComponent<WanderAI>());
                gameObject.AddComponent<SeekTowerAI>();
            }
            else if (!GetComponent<WanderAI>()) // If tower was there, but went null (didn't already have wander)
            {
                Destroy(GetComponent<SeekTowerAI>());
                gameObject.AddComponent<WanderAI>();
            }
        }
    }
}
