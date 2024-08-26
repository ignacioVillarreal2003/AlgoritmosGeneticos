using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private InitializeMovements initializeMovements;
    private GeneticAlgorithm geneticAlgorithm;
    private int actualMovement = -1;
    private GameObject player;
    public float playerSpeed = 5.0f;
    public float moveCooldown = 0.2f; 
    private float moveTimer = 0f;
    private Vector2 blockSize;

    void Start()
    {
        player = gameObject;
        initializeMovements = GetComponent<InitializeMovements>();
        blockSize = GetComponent<Collider2D>().bounds.size;
        geneticAlgorithm = FindAnyObjectByType<GeneticAlgorithm>();
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            actualMovement++;
            if (actualMovement < initializeMovements.GetNumberOfMovements()) {
                Move(initializeMovements.GetMovements()[actualMovement]);
            }
        }
    }

    void Move((int, int, int, int) movement)
    {
        if (movement.Item1 == 1){
            player.transform.position += new Vector3(0, 1 * (blockSize.y / 2), 0);
        } else if (movement.Item2 == 1){
            player.transform.position += new Vector3(1 * (blockSize.x / 2), 0, 0);
        } else if (movement.Item3 == 1){
            player.transform.position += new Vector3(0, -1 * (blockSize.y / 2), 0);
        } else if (movement.Item4 == 1){
            player.transform.position += new Vector3(-1 * (blockSize.x / 2), 0, 0);
        }
        moveTimer = moveCooldown;
    }

    public float TimeToEnd()
    {
        return moveCooldown * initializeMovements.GetNumberOfMovements() * 1.1f;
    }
}
