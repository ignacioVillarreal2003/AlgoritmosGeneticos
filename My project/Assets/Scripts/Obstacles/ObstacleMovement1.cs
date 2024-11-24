using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement1 : MonoBehaviour
{
    [SerializeField] private float distance = 40f;
    [SerializeField] private float speed = 2f; 
    [SerializeField] private bool toRight = true;
    private float startPosition = 0f;
    private float endPosition = 0f;      
    private float direction = 1f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        speed = speed / 10f;
        direction = 1f;
        InitializeMovement();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(speed * direction, 0, 0);
        transform.position += movement;

        if (transform.position.x > endPosition || transform.position.x < startPosition)
        {
            direction *= -1f;
        }
    }

    public void Reboot()
    {
        transform.position = initialPosition;
        InitializeMovement();
    }

    private void InitializeMovement()
    {
        if (!toRight) 
        {
            distance = Mathf.Abs(distance) * -1;
            speed = Mathf.Abs(speed) * -1;
        }
        else
        {
            distance = Mathf.Abs(distance);
            speed = Mathf.Abs(speed);
        }

        startPosition = transform.position.x;
        endPosition = transform.position.x + distance;

        if (startPosition > endPosition)
        {
            float temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;
        }
    }
}
