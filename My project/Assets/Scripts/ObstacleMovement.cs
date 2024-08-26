using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float distance = 6f;
    public float speed = 0.5f;    
    private float maxPosition;
    private float minPosition;
    private float direction = 1f;
    public bool isHorizontal = true;

    void Start()
    {
        speed = speed / 100f;
        if (isHorizontal) {
            maxPosition = transform.position.x + distance;
            minPosition = transform.position.x - distance;
        } else {
            maxPosition = transform.position.y + distance;
            minPosition = transform.position.y - distance;
        }
    }

    void Update()
    {
        if (isHorizontal){
            Vector3 movement = new Vector3(speed * direction, 0, 0);
            transform.position += movement;

            if (transform.position.x > maxPosition || transform.position.x < minPosition)
            {
                direction *= -1f;
            }
        } else {
            Vector3 movement = new Vector3(0, speed * direction, 0);
            transform.position += movement;

            if (transform.position.y > maxPosition || transform.position.y < minPosition)
            {
                direction *= -1f;
            }
        }
    }
}
