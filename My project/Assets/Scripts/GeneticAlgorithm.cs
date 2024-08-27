using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    [Header("Player options")]
    [SerializeField]
    private GameObject playerPrefab;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    private int numberOfPlayers = 20;
    
    [Header("Player movements")]
    [SerializeField]
    private int numberOfMovements = 100;
    [SerializeField]
    private float playerMoveCooldown = 0.2f;

    [Header("Checkpoints")]
    [SerializeField]
    private GameObject checkpoint;

    [Header("Generations")]
    [SerializeField]
    private int numGenerations = 20; 
    [SerializeField]
    private int numOfBest = 6;    
    private List<(List<(int, int, int, int)>, float)> generacion = new List<(List<(int, int, int, int)>, float)>();
    private int actualGeneration = 1;
    
    [Header("Other variables")]
    private float timeToFinishGeneration = 0;
    private bool canUpdate = true;

    void Start()
    {
        timeToFinishGeneration = playerMoveCooldown * numberOfMovements;
        GeneratePopulation();
    }

    void Update()
    {
        timeToFinishGeneration -= Time.deltaTime;
        if (timeToFinishGeneration < 0 && canUpdate)
        {
            UpdateData();
            canUpdate = false;
        }
    }

    public void UpdateData()
    {
        foreach (GameObject player in players)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement)
            {
                generacion.Add((playerMovement.GetMovements(), CalculateFitness(player, checkpoint)));
            }
        } 
        foreach ((List<(int, int, int, int)>, float) v in generacion)
        {
            Debug.Log(v.Item2);
        }
    }

    void AlgoritmoGenetico()
    {

    }

    void BestSelection()
    {

    }

    void Cross()
    {

    }

    void Mutate()
    {

    }

    public void NextGeneration()
    {
        DeletePopulation();
        actualGeneration++;
        GeneratePopulation();
        timeToFinishGeneration = playerMoveCooldown * numberOfMovements;
        canUpdate = true;
    }

    float CalculateFitness(GameObject player, GameObject checkpoint)
    {
        float distancia = Vector3.Distance(player.transform.position, checkpoint.transform.position);
        return distancia;
    }

    public List<(int, int, int, int)> GenerateMovements()
    {
        if (actualGeneration == 1)
        {
            List<(int, int, int, int)> movements = new List<(int, int, int, int)>();
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
            return movements;
        } else {
            List<(int, int, int, int)> movements = new List<(int, int, int, int)>();
            for (int i = 0; i < numberOfMovements; i++){
                float randomNumber = UnityEngine.Random.Range(0, 4);
                if (randomNumber == 0) {
                    movements.Add((1, 0, 0, 0));
                } else if (randomNumber == 1) {
                    movements.Add((0, 1, 0, 0));
                } else if (randomNumber == 2) {
                    movements.Add((0, 0, 1, 0));
                } else {
                    movements.Add((0, 0, 1, 0));
                }
            }
            return movements;
        }
    }
    
    void DeletePopulation()
    {
        foreach (GameObject p in players)
        {
            Destroy(p);
        }
        players.Clear();
    }

    void GeneratePopulation()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            players.Add(player);
        }
    }

    public float GetPlayerMoveCooldown()
    {
        return playerMoveCooldown;
    }

    public float GetActualGeneration()
    {
        return actualGeneration;
    }
}
