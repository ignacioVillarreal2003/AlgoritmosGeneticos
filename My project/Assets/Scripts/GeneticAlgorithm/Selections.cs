using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selections : MonoBehaviour
{
    /* En este método, cada individuo tiene una probabilidad de ser seleccionado 
    que es proporcional a su aptitud. Se puede imaginar como una 
    ruleta en la que cada individuo tiene un sector de la rueda cuyo tamaño 
    es proporcional a su aptitud. Los individuos con mayor aptitud tienen más 
    probabilidad de ser seleccionados, pero aún existe la posibilidad de que 
    individuos con menor aptitud sean elegidos. */
    public PlayerController RouletteWheelSelection(List<PlayerController> population)
    {
        float totalFitness = population.Sum(p => p.getFitness());
        float randomValue = Random.Range(0, totalFitness);
        float cumulativeFitness = 0f;

        foreach (PlayerController player in population)
        {
            cumulativeFitness += player.getFitness();
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
    public PlayerController TournamentSelection(List<PlayerController> population, int capacity)
    {
        List<PlayerController> tournamentPopulation = new List<PlayerController>();
        
        for (int i = 0; i < capacity; i++)
        {
            int index = Random.Range(0, population.Count);
            tournamentPopulation.Add(population[index]);
        }

        return tournamentPopulation.OrderByDescending(p => p.getFitness()).First();
    }

    /* Igual pero con torneos donde varian los integrantes */
    public PlayerController RandomTournamentSelection(List<PlayerController> population)
    {
        int capacity = Random.Range(2, Mathf.CeilToInt(population.Count * 0.1f));
        List<PlayerController> tournamentPopulation = new List<PlayerController>();
        
        for (int i = 0; i < capacity; i++)
        {
            PlayerController selected = population[Random.Range(0, population.Count)];
            tournamentPopulation.Add(selected);
        }

        return tournamentPopulation.OrderByDescending(p => p.getFitness()).First();
    }

    /* En lugar de elegir siempre al mejor en cada torneo, se elige de manera 
    probabilística entre los individuos del torneo, lo que permite que los 
    individuos con menor aptitud también puedan ser seleccionados, aunque con 
    menos probabilidad. */
    public PlayerController StochasticTournamentSelection(List<PlayerController> population, int capacity)
    {
        List<PlayerController> tournamentPopulation = new List<PlayerController>();
        
        for (int i = 0; i < capacity; i++)
        {
            int index = Random.Range(0, population.Count);
            tournamentPopulation.Add(population[index]);
        }

        int finalSelection = Random.Range(0, tournamentPopulation.Count);
        
        return tournamentPopulation[finalSelection];
    }

    /* La selección elitista asegura que los mejores individuos de la población 
    actual se copien directamente a la siguiente generación sin sufrir modificaciones. */
    public List<PlayerController> ElitismSelection(List<PlayerController> population, int eliteCount)
    {
        return population.OrderByDescending(p => p.getFitness()).Take(eliteCount).ToList();
    }

    /* En lugar de usar directamente la aptitud para determinar las probabilidades 
    de selección, los individuos se ordenan en función de su aptitud, y la 
    probabilidad de selección se basa en el rango. */
    public PlayerController RankSelection(List<PlayerController> population, int capacity)
    {
        List<PlayerController> rankedPopulation = population.OrderBy(p => p.getFitness()).ToList();
        
        float totalRank = capacity * (capacity + 1) / 2f;
        float randomValue = Random.Range(0, totalRank);
        float cumulativeRank = 0f;

        for (int i = 0; i < rankedPopulation.Count; i++)
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
    public PlayerController TruncationSelection(List<PlayerController> population, float truncationFactor)
    {
        truncationFactor = Mathf.Clamp(truncationFactor, 1f, 3.8f);
        float totalFitness = population.Sum(p => p.getFitness());
        float randomValue = Random.Range(totalFitness / (truncationFactor + 0.2f), totalFitness);
        float cumulativeFitness = 0f;

        foreach (PlayerController player in population)
        {
            cumulativeFitness += player.getFitness();
            if (randomValue <= cumulativeFitness)
            {
                return player;
            }
        }

        return population[population.Count - 1];
    }
}
