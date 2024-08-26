using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private List<(int, int, int, int)> movements = new List<(int, int, int, int)>();
    public int numberOfMovements = 100;
    private int actualMovement = -1;
    public GameObject meta;
    public GameObject player;
    public float playerSpeed = 5.0f;
    public float moveCooldown = 0.2f; 
    private float moveTimer = 0f;
    private Vector2 blockSize;

    void Start()
    {
        player = gameObject;
        blockSize = GetComponent<Collider2D>().bounds.size;
        GenerateMovements();
        meta = GameObject.FindGameObjectWithTag("End");
    }

    void GenerateMovements()
    {
        for (int i = 0; i < numberOfMovements; i++){
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
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            actualMovement++;
            if (actualMovement < 100) {
                Move(movements[actualMovement]);
            } else {
                float distancia = Vector3.Distance(player.transform.position, meta.transform.position);
                Debug.Log("Fitnes: " + distancia);
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
}
