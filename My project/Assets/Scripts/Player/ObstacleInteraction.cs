using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleInteraction : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            Destroy(gameObject); 
        }
    }
}
