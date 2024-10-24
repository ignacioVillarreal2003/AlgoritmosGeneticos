using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticController : MonoBehaviour
{
    /* Generacion */
    private int currentGeneration = 1; 
    [Range(10, 100)][SerializeField] private int populationSize = 10;
    private List<PlayerController> population = new List<PlayerController>();

    /* Poblacion */
    [Range(20, 100)] [SerializeField] private int chromosomeLength = 20;
    [SerializeField] private float moveCooldown = 0.2f;
    [SerializeField] private int velocity = 5;
    [SerializeField] private float penality = 0.1f;
    [SerializeField] private GameObject playerPrefab;

    /* Algoritmo */
    private Selections selections;
    private Crosses crosses;
    private Mutations mutations;

    /* Entorno */
    private ObstaclesManager obstaclesManager;
    private CheckpointsManager checkpointsManager;

    /* Otros */
    private bool simulationFinished = false;
    private float bestFitness = 0f;

    /* Referencias a objetos */
    void Awake()
    {
        selections = FindAnyObjectByType<Selections>();
        crosses = FindAnyObjectByType<Crosses>();
        mutations = FindAnyObjectByType<Mutations>();
        obstaclesManager = FindAnyObjectByType<ObstaclesManager>();
        checkpointsManager = FindAnyObjectByType<CheckpointsManager>();
    }
    
    void Start()
    {
        /* Inicializamos la poblacion inicial */
        for (int i = 0; i < populationSize; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            PlayerController player = playerObj.GetComponent<PlayerController>();
            player.InitializeGenes();
            population.Add(player);
        }

        /* Cargamos los checkpoints en los individuos */
        checkpointsManager.LoadCheckpoints();
    }

    void Update()
    {
        if (!simulationFinished)
        {
            /* Si uno llega a la meta terminamos la simulacion */
            bool onePlayerIsFinished = population.Any(p => p.GetIsFinish());
            if (onePlayerIsFinished) 
            {
                simulationFinished = true;
            }
            /* Si todos murieron creamos una nueva poblacion */
            bool allPlayersAreDead = population.All(p => p.GetIsDead());
            Debug.Log(allPlayersAreDead); // raro a veces no entra aca
            if (allPlayersAreDead)
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
        /* Analisis de la poblacion anterior */
        population.Sort((a, b) => a.GetFitness().CompareTo(b.GetFitness()));
        if (population[population.Count - 1].GetFitness() > bestFitness)
        {
            bestFitness = population[population.Count - 1].GetFitness();
        }

        /* Añadimos nuevos individuos a la poblacion mediante seleccion-cruce-mutacion */
        List<PlayerController> newPopulation = new List<PlayerController>();

        while (newPopulation.Count < populationSize)
        {
            PlayerController parent1 = selections.RouletteWheelSelection(population); 
            PlayerController parent2 = selections.RouletteWheelSelection(population);
            ((int, int, int, int)[], (int, int, int, int)[]) childs = crosses.SinglePointCrossover(parent1.GetChromosome(), parent2.GetChromosome()); 
            (int, int, int, int)[] chromosome1 = mutations.UniformMutation(childs.Item1);
            (int, int, int, int)[] chromosome2 = mutations.UniformMutation(childs.Item2);

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

        /* Remplazamos la poblacion anterior con la nueva */
        foreach (var player in population)
        {
            Destroy(player.gameObject);
        }
        currentGeneration ++;
        population = newPopulation;

        /* Reiniciamos los obstaculos y checkpoints */
        if (obstaclesManager)
        {
            obstaclesManager.RebootAll();
        }
        checkpointsManager.LoadCheckpoints();
    }
    
    public List<PlayerController> GetPopulation() => population;
    public int GetChromosomeLength() => chromosomeLength;
    public int GetCurrentGeneration() => currentGeneration;
    public float GetMoveCooldown() => moveCooldown;
    public int GetVelocity() => velocity;
    public float GetPenality() => penality;
    public float GetBestFitness() => bestFitness;
    public int GetPopulationSize() => populationSize;
}