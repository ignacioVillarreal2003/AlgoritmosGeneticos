using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneticController : MonoBehaviour
{
    /* Generacion */
    private int currentGeneration = 1; 
    [Range(20, 100)] [SerializeField] private int populationSize = 20;
    private List<PlayerController> population = new List<PlayerController>();

    /* Poblacion */
    [Range(20, 100)] [SerializeField] private int chromosomeLength = 40;
    [SerializeField] private float moveCooldown = 0.2f;
    [SerializeField] private int velocity = 5;
    [SerializeField] private float penality = 0.10f;
    [SerializeField] private GameObject playerPrefab;

    /* Algoritmo */
    private Selections selections;
    [SerializeField] private Selections.SelectionsOptions selectionOptions = Selections.SelectionsOptions.TruncationSelection;
    private Crosses crosses;
    [SerializeField] private Crosses.CrossesOptions crossesOptions = Crosses.CrossesOptions.SinglePointCrossover;
    private Mutations mutations;
    [SerializeField] private Mutations.MutationsOptions mutationsOptions = Mutations.MutationsOptions.UniformMutation;

    /* Entorno */
    private ObstaclesManager obstaclesManager;
    private CheckpointsManager checkpointsManager;
    private Checkpoint lastCheckpoint;

    /* Otros */
    private bool simulationFinished = false;
    private float bestDistance = 10000f;
    private float actualDistance = 0;
    private Logger logger;

    /* Referencias a objetos */
    void Awake()
    {
        selections = FindAnyObjectByType<Selections>();
        crosses = FindAnyObjectByType<Crosses>();
        mutations = FindAnyObjectByType<Mutations>();
        obstaclesManager = FindAnyObjectByType<ObstaclesManager>();
        checkpointsManager = FindAnyObjectByType<CheckpointsManager>();
        logger = FindAnyObjectByType<Logger>();
    }
    
    void Start()
    {
        LevelController levelController = FindObjectOfType<LevelController>();
        if (levelController != null)
        {
            populationSize = levelController.GetPopulationSize();
            chromosomeLength = levelController.GetChromosomeLength();
            selectionOptions = levelController.GetSelection();
            mutationsOptions = levelController.GetMutation();
            crossesOptions = levelController.GetCross();
        }
        /* Inicializamos la poblacion inicial */
        for (int i = 0; i < populationSize; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            PlayerController player = playerObj.GetComponent<PlayerController>();
            player.InitializeGenes();
            population.Add(player);
        }

        /* Logs de datos */
        logger.InitialData();
    }

    void Update()
    {
        if (!simulationFinished)
        {
            /* Si uno llega a la meta terminamos la simulacion */
            if (population.Any(p => p.GetIsFinish())) 
            {
                simulationFinished = true;
                ClearPopulation();
                
                /* Logs de datos */
                logger.FinalData();
                SceneManager.LoadScene(0);
            }

            /* Si todos murieron creamos una nueva poblacion */
            if (population.All(p => p.GetIsDead()))
            {
                CreateNewPopulation();
            }
        } 
        else 
        {
            /* Finalizamos la simulacion */
            return;
        }        
    }

    void CreateNewPopulation()
    {
        CalculateFitness();
        logger.LogGenerationStats();
        PopulationProcessing();
        /* Añadimos nuevos individuos a la poblacion mediante seleccion-cruce-mutacion */
        List<PlayerController> newPopulation = new List<PlayerController>();
        List<(int, int, int, int)[]> elitChromosomes = new List<(int, int, int, int)[]>();

        List<PlayerController> elitPopulation = selections.ElitismSelection(population); 
        elitPopulation.ForEach((PlayerController p) => { 
            elitChromosomes.Add(p.GetChromosome());
        });

        foreach ((int, int, int, int)[] chromosome in elitChromosomes)
        {
            GameObject playerObj = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            PlayerController individual = playerObj.GetComponent<PlayerController>();
            individual.SetChromosome(chromosome);
            newPopulation.Add(individual);
        }
    
        while (newPopulation.Count < populationSize)
        {
            PlayerController parent1 = selections.Select(selectionOptions, population); 
            PlayerController parent2 = selections.Select(selectionOptions, population);
            ((int, int, int, int)[], (int, int, int, int)[]) childs = crosses.Cross(crossesOptions, parent1.GetChromosome(), parent2.GetChromosome());
            (int, int, int, int)[] chromosome1 = mutations.Mutate(mutationsOptions, childs.Item1);
            (int, int, int, int)[] chromosome2 = mutations.Mutate(mutationsOptions, childs.Item2);

            if (newPopulation.Count < populationSize)
            {
                GameObject playerObj1 = Instantiate(playerPrefab, transform.position, Quaternion.identity);
                PlayerController child1 = playerObj1.GetComponent<PlayerController>();
                child1.SetChromosome(chromosome1);
                newPopulation.Add(child1);
            }
            if (newPopulation.Count < populationSize)
            {
                GameObject playerObj2 = Instantiate(playerPrefab, transform.position, Quaternion.identity);
                PlayerController child2 = playerObj2.GetComponent<PlayerController>();
                child2.SetChromosome(chromosome2);
                newPopulation.Add(child2);
            }
        }

        /* Ajustamos datos */
        ClearPopulation();
        currentGeneration ++;
        population = newPopulation;
        ResetEnvironment();
    }    

    /* Analisis de la poblacion */
    private void PopulationProcessing()
    {
        population.Sort((a, b) => a.GetFitness().CompareTo(b.GetFitness())); // Menor a mayor
        if (population[population.Count - 1].GetDistanceToTarget() < bestDistance)
        {
            bestDistance = population[population.Count - 1].GetDistanceToTarget();
            mutations.ChangeMutationRateAdaptative(true);
        }
        else
        {
            mutations.ChangeMutationRateAdaptative(false);
        }
        actualDistance = population[population.Count - 1].GetDistanceToTarget();
    }

    /* Elimina la poblacion */
    private void ClearPopulation()
    {
        foreach (var player in population)
        {
            Destroy(player.gameObject);
        }
    }

    /* Reiniciamos los obstaculos y checkpoints */
    private void ResetEnvironment()
    {
        if (obstaclesManager)
        {
            obstaclesManager.RebootAll();
        }
    }

    private void CalculateFitness()
    {
        lastCheckpoint = checkpointsManager.GetLastCheckpoint();
        float minDistance = float.MaxValue;
        float maxDistance = float.MinValue;

        foreach (var player in population)
        {
            float distance = Mathf.Abs(Vector3.Distance(player.transform.position, lastCheckpoint.transform.position));
            player.SetDistanceToTarget(distance);
            minDistance = Mathf.Min(minDistance, distance);
            maxDistance = Mathf.Max(maxDistance, distance);
        }

        foreach (var player in population)
        {
            float normalizedFitness = 1f - ((player.GetDistanceToTarget() - minDistance) / (maxDistance - minDistance));
            normalizedFitness = Mathf.Clamp(normalizedFitness, 0f, 1f);

           int visitedCheckpoints = player.GetVisitedCheckpoints();
            normalizedFitness += normalizedFitness * visitedCheckpoints * penality;

            if (player.GetIsDead())
            {
                normalizedFitness -= normalizedFitness * penality;
            }

            normalizedFitness = Mathf.Clamp(normalizedFitness, 0f, 1f);

            player.SetFitness(normalizedFitness);
        }
    }
    
    public List<PlayerController> GetPopulation() => population;
    public int GetChromosomeLength() => chromosomeLength;
    public int GetCurrentGeneration() => currentGeneration;
    public float GetMoveCooldown() => moveCooldown;
    public int GetVelocity() => velocity;
    public float GetPenality() => penality;
    public float GetBestDistance() => bestDistance;
    public float GetActualDistance() => actualDistance;
    public int GetPopulationSize() => populationSize;
    public Selections.SelectionsOptions GetSelectionOptions() => selectionOptions;
    public Crosses.CrossesOptions GetCrossesOptions() => crossesOptions;
    public Mutations.MutationsOptions GetMutationsOptions() => mutationsOptions;
}