using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleInteraction : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = gameObject;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Obstacle"))
        {
            Destroy(player);
        }
    }
}
