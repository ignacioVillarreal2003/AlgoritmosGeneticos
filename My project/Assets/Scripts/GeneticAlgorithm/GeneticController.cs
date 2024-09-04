using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticController : MonoBehaviour
{
    private int currentGeneration = 1; 
    private List<PlayerController> population = new List<PlayerController>();
    [Range(40, 200)] [SerializeField] private int adnLength = 40;
    [SerializeField] private float moveCooldown = 0.2f;
    [Range(10, 50)][SerializeField] private int populationSize = 10;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float mutationRate;
    private float generationTime = 0;
    private Selections selections;
    private Crosses crosses;
    private Mutations mutations;
    private float bestFitness = 0f;

    void Awake()
    {
        selections = FindAnyObjectByType<Selections>();
        crosses = FindAnyObjectByType<Crosses>();
        mutations = FindAnyObjectByType<Mutations>();
    }
    
    void Start()
    {
        populationSize *= 2;
        InitializePopulation();
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

            GameObject playerObj2 = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            PlayerController child2 = playerObj2.GetComponent<PlayerController>();
            child2.InitializeGenes(newAdn.Item2);
            mutations.NormalMutation(child2);

            newPopulation.Add(child1);
            newPopulation.Add(child2);
        }

        foreach (var player in population)
        {
            Destroy(player.gameObject);
        }

        population = newPopulation;
    }

    public List<PlayerController> getPopulation()
    {
        return population;
    }

    public int getAdnLength()
    {
        return adnLength;
    }

    public float getMutationRate()
    {
        return mutationRate;
    }

    public int getCurrentGeneration()
    {
        return currentGeneration;
    }

    public float getMoveCooldown()
    {
        return moveCooldown;
    }

    public float getGenerationTime()
    {
        return generationTime;
    }

    public float getBestFitness()
    {
        return bestFitness;
    }  

    public int getPopulationSize()
    {
        return populationSize;
    }    
}
