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
    [SerializeField] private GameObject playerPrefab;


    /* Algoritmo */
    private Selections selections;
    private Crosses crosses;
    private Mutations mutations;

    /* Otros */
    private float generationTime = 0;
    private float bestFitness = 0f;
    private ObstaclesManager obstaclesManager;
    private CheckpointsManager checkpointsManager;

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

        generationTime = moveCooldown * chromosomeLength + 1f;
        checkpointsManager.InitializePlayers();
    }

    void Update()
    {
        if (OnePlayerIsFinished())
        {
            return;
        }
        if (AllPlayersAreDead())
        {
            CreateNewPopulation();
        }
        generationTime -= Time.deltaTime;
        if (generationTime < 0)
        {
            foreach(PlayerController p in population)
            {
                p.isDead = true;
            }
            generationTime = moveCooldown * chromosomeLength + 1f;
        }
    }
    bool AllPlayersAreDead()
    {
        return population.All(p => p.isDead);
    }

    bool OnePlayerIsFinished()
    {
        foreach(PlayerController p in population)
        {
            if (p.isFinish)
            {
                return true;
            }
        }
        return false;
    }

    void CreateNewPopulation()
    {
        List<PlayerController> newPopulation = new List<PlayerController>();
        population.Sort((a, b) => a.getFitness().CompareTo(b.getFitness()));
        if (population[population.Count - 1].getFitness() > bestFitness)
        {
            bestFitness = population[population.Count - 1].getFitness();
        }

        /* Añadimos nuevos individuos a la poblacion mediante seleccion-cruce-mutacion */
        while (newPopulation.Count < populationSize)
        {
            PlayerController parent1 = selections.RouletteWheelSelection(population); 
            PlayerController parent2 = selections.RouletteWheelSelection(population);
            ((int, int, int, int)[], (int, int, int, int)[]) newAdn = crosses.SinglePointCrossover(parent1, parent2); 

            if (newPopulation.Count < populationSize)
            {
                GameObject playerObj1 = Instantiate(playerPrefab, transform.position, Quaternion.identity);
                PlayerController child1 = playerObj1.GetComponent<PlayerController>();
                child1.InitializeGenes(newAdn.Item1);
                mutations.UniformMutation(child1);
                newPopulation.Add(child1);
            }
            if (newPopulation.Count < populationSize)
            {
                GameObject playerObj2 = Instantiate(playerPrefab, transform.position, Quaternion.identity);
                PlayerController child2 = playerObj2.GetComponent<PlayerController>();
                child2.InitializeGenes(newAdn.Item2);
                mutations.UniformMutation(child2);
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
        checkpointsManager.InitializePlayers();
    }
    
    public List<PlayerController> getPopulation() => population;
    public int getChromosomeLength() => chromosomeLength;
    public int getCurrentGeneration() => currentGeneration;
    public float getMoveCooldown() => moveCooldown;
    public int getVelocity() => velocity;
    public float getGenerationTime() => generationTime;
    public float getBestFitness() => bestFitness;
    public int getPopulationSize() => populationSize;
}