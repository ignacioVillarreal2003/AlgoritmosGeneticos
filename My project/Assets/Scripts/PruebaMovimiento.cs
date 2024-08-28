using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaMovimiento : MonoBehaviour
{
    private List<(int, int, int, int)> movements = new List<(int, int, int, int)>();
    private int actualMovement = -1;
    public float moveCooldown = 0.05f;
    private float moveTimer = 0;
    private Vector2 blockSize;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveProgress = 0f;
    public float moveDuration = 0.05f; 

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            float randomNumber = UnityEngine.Random.Range(0, 4);
            if (randomNumber == 0) {
                movements.Add((1, 0, 0, 0));
            } else if (randomNumber == 1) {
                movements.Add((0, 1, 0, 0));
            } else if (randomNumber == 2) {
                movements.Add((0, 0, 1, 0));
            } else {
                movements.Add((0, 0, 0, 1));
            }
        }
        blockSize = GetComponent<Collider2D>().bounds.size;
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update()
    {
        if (moveProgress < 1f)
        {
            moveProgress += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
        }
        else
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                actualMovement++;
                if (actualMovement < movements.Count)
                {
                    Move(movements[actualMovement]);
                }
            }
        }
    }

    void Move((int, int, int, int) movement)
    {
        startPosition = transform.position;
        if (movement.Item1 == 1)
        {
            targetPosition = startPosition + new Vector3(0, blockSize.y / 2, 0);
        }
        else if (movement.Item2 == 1)
        {
            targetPosition = startPosition + new Vector3(blockSize.x / 2, 0, 0);
        }
        else if (movement.Item3 == 1)
        {
            targetPosition = startPosition + new Vector3(0, -blockSize.y / 2, 0);
        }
        else if (movement.Item4 == 1)
        {
            targetPosition = startPosition + new Vector3(-blockSize.x / 2, 0, 0);
        }

        moveProgress = 0f;
        moveTimer = moveCooldown;
    }
}
