using System;
using System.Collections.Generic;
using UnityEngine;
public class IndividualMovement : MonoBehaviour
{
    private GeneticAlgorithm geneticAlgorithm;

    [Header("Movements")]
    private List<(int, int, int, int)> movements = new List<(int, int, int, int)>();
    private int actualMovement = -1;
    private float moveTimer = 0f;
    private Vector2 blockSize;

    void Start()
    {
        geneticAlgorithm = FindAnyObjectByType<GeneticAlgorithm>();
        movements = geneticAlgorithm.GetMovements();
        blockSize = GetComponent<Collider2D>().bounds.size;
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            actualMovement++;
            if (actualMovement < movements.Count) {
                Move(movements[actualMovement]);
            }
        }
    }

    void Move((int, int, int, int) movement)
    {
        if (movement.Item1 == 1){
            gameObject.transform.position += new Vector3(0, 1 * (blockSize.y / 4), 0);
        } else if (movement.Item2 == 1){
            gameObject.transform.position += new Vector3(1 * (blockSize.x / 4), 0, 0);
        } else if (movement.Item3 == 1){
            gameObject.transform.position += new Vector3(0, -1 * (blockSize.y / 4), 0);
        } else if (movement.Item4 == 1){
            gameObject.transform.position += new Vector3(-1 * (blockSize.x / 4), 0, 0);
        }
        moveTimer = geneticAlgorithm.GetPlayerMoveCooldown();
    }

    public List<(int, int, int, int)> GetMovements()
    {
        return movements;
    }
}
