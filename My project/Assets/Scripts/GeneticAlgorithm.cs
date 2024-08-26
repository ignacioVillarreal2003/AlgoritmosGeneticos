using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    private int numGeneraciones = 20; 
    private int numMejores = 6;    
    private InitializePopulation initializePopulation;
    private List<(List<(int, int, int, int)>, float)> generacion = new List<(List<(int, int, int, int)>, float)>();
    private float time = 0.2f * 100f;
    private bool canUpdate = true;

    void Start()
    {
        initializePopulation = FindAnyObjectByType<InitializePopulation>();
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0 && canUpdate)
        {
            UpdateData();
            canUpdate = false;
        }
    }

    public void UpdateData()
    {
        List<GameObject> players = initializePopulation.GetPlayers();
        foreach (GameObject player in players)
        {
            InitializeMovements playerMovement = player.GetComponent<InitializeMovements>();
            Fitness playerFitness = player.GetComponent<Fitness>();
            if (playerMovement && playerFitness)
            {
                generacion.Add((playerMovement.GetMovements(), playerFitness.CalcularFitness()));
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
}
