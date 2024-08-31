using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("Values")]
    [SerializeField]
    private float distance = 40f;

    [SerializeField]
    private float speed = 2f; 

    [SerializeField]
    private bool toRight = true;
    private float initialPosition = 0f;
    private float finalPosition = 0f;      
    private float direction = 1f;
    

    void Start()
    {
        if (!toRight) 
        {
            distance = distance * -1;
            speed = speed * -1;
        }
        speed = speed / 100f;
        initialPosition = transform.position.x;
        finalPosition = transform.position.x + distance;

        if (initialPosition > finalPosition)
        {
            float temp = initialPosition;
            initialPosition = finalPosition;
            finalPosition = temp;
        }
    }

    void Update()
    {
        Vector3 movement = new Vector3(speed * direction, 0, 0);
        transform.position += movement;

        if (transform.position.x > finalPosition || transform.position.x < initialPosition)
        {
            direction *= -1f;
        }
         
    }
}
