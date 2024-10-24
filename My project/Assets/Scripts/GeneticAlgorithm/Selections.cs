using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selections : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] private int eliteCount = 0;
    [Range(1.2f, 4f)] [SerializeField] private float factorTruncationSelection = 1.3f;
    [Range(2, 10)] [SerializeField] private int capacityTournamentSelection = 2;
    [Range(2, 10)] [SerializeField] private int capacityStochasticTournamentSelection = 2;

    public PlayerController Select(SelectionsOptions selectionOptions, List<PlayerController> population)
    {
        if (selectionOptions == SelectionsOptions.RouletteWheelSelection)
        {
            return RouletteWheelSelection(population); 
        }
        if (selectionOptions == SelectionsOptions.TournamentSelection)
        {
            return TournamentSelection(population); 
        }
        if (selectionOptions == SelectionsOptions.RandomTournamentSelection)
        {
            return RandomTournamentSelection(population); 
        }
        if (selectionOptions == SelectionsOptions.StochasticTournamentSelection)
        {
            return StochasticTournamentSelection(population); 
        }
        if (selectionOptions == SelectionsOptions.TruncationSelection)
        {
            return TruncationSelection(population); 
        }
        if (selectionOptions == SelectionsOptions.RankSelection)
        {
            return RankSelection(population); 
        }
        return population.Last();
    }

    public enum SelectionsOptions
    {
        RouletteWheelSelection,
        TournamentSelection,
        RandomTournamentSelection,
        StochasticTournamentSelection,
        TruncationSelection,
        RankSelection
    }

    /* La selección elitista asegura que los mejores individuos de la población 
    actual se copien directamente a la siguiente generación sin sufrir modificaciones. */
    public List<PlayerController> ElitismSelection(List<PlayerController> population)
    {
        return population.OrderByDescending(p => p.GetFitness()).Take(eliteCount).ToList();
    }

    /* En este método, cada individuo tiene una probabilidad de ser seleccionado 
    que es proporcional a su aptitud. Se puede imaginar como una 
    ruleta en la que cada individuo tiene un sector de la rueda cuyo tamaño 
    es proporcional a su aptitud. Los individuos con mayor aptitud tienen más 
    probabilidad de ser seleccionados, pero aún existe la posibilidad de que 
    individuos con menor aptitud sean elegidos. */
    private PlayerController RouletteWheelSelection(List<PlayerController> population)
    {
        float totalFitness = population.Sum(p => p.GetFitness());
        float randomValue = Random.Range(0, totalFitness);
        float cumulativeFitness = 0f;

        foreach (PlayerController player in population)
        {
            cumulativeFitness += player.GetFitness();
            if (randomValue <= cumulativeFitness)
            {
                return player;
            }
        }

        return population.Last();
    }

    /* En este método, se seleccionan aleatoriamente varios individuos 
    (de un subconjunto de la población), y de estos se elige el que tiene 
    la mayor aptitud. */
    private PlayerController TournamentSelection(List<PlayerController> population)
    {
        List<PlayerController> tournamentPopulation = new List<PlayerController>();
        
        for (int i = 0; i < capacityTournamentSelection; i++)
        {
            int index = Random.Range(0, population.Count);
            tournamentPopulation.Add(population[index]);
        }

        return tournamentPopulation.OrderByDescending(p => p.GetFitness()).First();
    }

    /* Igual pero con torneos donde varian los integrantes */
    private PlayerController RandomTournamentSelection(List<PlayerController> population)
    {
        int capacity = Random.Range(2, 10);
        List<PlayerController> tournamentPopulation = new List<PlayerController>();
        
        for (int i = 0; i < capacity; i++)
        {
            PlayerController selected = population[Random.Range(0, population.Count)];
            tournamentPopulation.Add(selected);
        }

        return tournamentPopulation.OrderByDescending(p => p.GetFitness()).First();
    }

    /* En lugar de elegir siempre al mejor en cada torneo, se elige de manera 
    probabilística entre los individuos del torneo, lo que permite que los 
    individuos con menor aptitud también puedan ser seleccionados, aunque con 
    menos probabilidad. */
    private PlayerController StochasticTournamentSelection(List<PlayerController> population)
    {
        List<PlayerController> tournamentPopulation = new List<PlayerController>();
        
        for (int i = 0; i < capacityStochasticTournamentSelection; i++)
        {
            int index = Random.Range(0, population.Count);
            tournamentPopulation.Add(population[index]);
        }

        int finalSelection = Random.Range(0, tournamentPopulation.Count);
        
        return tournamentPopulation[finalSelection];
    }

    /* En lugar de usar directamente la aptitud para determinar las probabilidades 
    de selección, los individuos se ordenan en función de su aptitud, y la 
    probabilidad de selección se basa en el rango. */
    private PlayerController RankSelection(List<PlayerController> population)
    {
        List<PlayerController> rankedPopulation = population.OrderBy(p => p.GetFitness()).ToList();
        
        int populationSize = rankedPopulation.Count;
        float totalRankSum = populationSize * (populationSize + 1) / 2f;
        
        float randomValue = Random.Range(0, totalRankSum);
        float cumulativeRank = 0f;

        for (int i = 0; i < populationSize; i++)
        {
            cumulativeRank += i + 1;
            
            if (randomValue <= cumulativeRank)
            {
                return rankedPopulation[i];
            }
        }

        return rankedPopulation.Last();
    }


    /* En la selección truncada, solo se selecciona a los individuos más aptos 
    (por ejemplo, el 50% superior) para reproducirse. El número de individuos 
    que se selecciona se llama "tasa de truncamiento". */
    private PlayerController TruncationSelection(List<PlayerController> population)
    {
        float totalFitness = population.Sum(p => p.GetFitness());
        float randomValue = Random.Range(totalFitness / factorTruncationSelection, totalFitness);
        float cumulativeFitness = 0f;

        foreach (PlayerController player in population)
        {
            cumulativeFitness += player.GetFitness();
            if (randomValue <= cumulativeFitness)
            {
                return player;
            }
        }

        return population.Last();
    }
}
