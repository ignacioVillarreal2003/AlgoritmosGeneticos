using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticController : MonoBehaviour
{
    private int currentGeneration = 1; 
    private List<PlayerController> population = new List<PlayerController>();
    [Range(20, 100)] [SerializeField] private int adnLength = 20;
    [SerializeField] private float moveCooldown = 0.2f;
    [SerializeField] private int velocity = 5;
    [Range(5, 50)][SerializeField] private int populationSize = 5;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float mutationRate = 0.02f;
    private float generationTime = 0;
    private Selections selections;
    private Crosses crosses;
    private Mutations mutations;
    private float bestFitness = 0f;
    private ObstaclesManager obstaclesManager;

    void Awake()
    {
        selections = FindAnyObjectByType<Selections>();
        crosses = FindAnyObjectByType<Crosses>();
        mutations = FindAnyObjectByType<Mutations>();
        obstaclesManager = FindAnyObjectByType<ObstaclesManager>();
    }
    
    void Start()
    {
        populationSize *= 2;
        InitializePopulation();
        generationTime = moveCooldown * adnLength + 1f;
    }

    void Update()
    {
        if (AllPlayersAreDead())
        {
            BreedNewPopulation();
        }
        generationTime -= Time.deltaTime;
        if (generationTime < 0)
        {
            foreach(PlayerController p in population)
            {
                p.isDead = true;
            }
            generationTime = moveCooldown * adnLength + 1f;
        }
    }

    void InitializePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject playerObj = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            PlayerController player = playerObj.GetComponent<PlayerController>();
            player.InitializeGenes();
            population.Add(player);
        }
    }

    bool AllPlayersAreDead()
    {
        return population.All(p => p.isDead);
    }

    void BreedNewPopulation()
    {
        List<PlayerController> newPopulation = new List<PlayerController>();
        population.Sort((a, b) => a.getFitness().CompareTo(b.getFitness()));
        if (population[population.Count - 1].getFitness() > bestFitness)
        {
            bestFitness = population[population.Count - 1].getFitness();
        }

        for (int i = 0; i < populationSize / 2; i++)
        {
            PlayerController parent1 = selections.NormalSelection(); 
            PlayerController parent2 = selections.NormalSelection();
            ((int, int, int, int)[], (int, int, int, int)[]) newAdn = crosses.NormalCrossOver(parent1, parent2); 

            GameObject playerObj1 = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            PlayerController child1 = playerObj1.GetComponent<PlayerController>();
            child1.InitializeGenes(newAdn.Item1);
            mutations.NormalMutation(child1);
            newPopulation.Add(child1);

            GameObject playerObj2 = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            PlayerController child2 = playerObj2.GetComponent<PlayerController>();
            child2.InitializeGenes(newAdn.Item2);
            mutations.NormalMutation(child2);
            newPopulation.Add(child2);
        }

        foreach (var player in population)
        {
            Destroy(player.gameObject);
        }
        currentGeneration ++;
        population = newPopulation;

        if (obstaclesManager)
        {
            obstaclesManager.RebootAll();
        }
    }
    public List<PlayerController> getPopulation() => population;
    public int getAdnLength() => adnLength;
    public float getMutationRate() => mutationRate;
    public int getCurrentGeneration() => currentGeneration;
    public float getMoveCooldown() => moveCooldown;
    public int getVelocity() => velocity;
    public float getGenerationTime() => generationTime;
    public float getBestFitness() => bestFitness;
    public int getPopulationSize() => populationSize;
}