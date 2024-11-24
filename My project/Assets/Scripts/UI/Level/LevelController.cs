using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int level = 3;
    [SerializeField] private int populationSize = 20;
    [SerializeField] private int chromosomeLength = 20;
    [SerializeField] private int eliteCount = 0;
    [SerializeField] private Selections.SelectionsOptions selection = Selections.SelectionsOptions.RouletteWheelSelection;
    [SerializeField] private Mutations.MutationsOptions mutation = Mutations.MutationsOptions.UniformMutation;
    [SerializeField] private Crosses.CrossesOptions cross = Crosses.CrossesOptions.SinglePointCrossover;
    public static LevelController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        level = 3;
        populationSize = 20;
        chromosomeLength = 20;
        eliteCount = 0;
    }

    public int GetLevel() => level;
    public int GetPopulationSize() => populationSize;
    public int GetChromosomeLength() => chromosomeLength;
    public int GetEliteCount() => eliteCount;
    public Selections.SelectionsOptions GetSelection() => selection;
    public Mutations.MutationsOptions GetMutation() => mutation;
    public Crosses.CrossesOptions GetCross() => cross;


    public void SetLevel(int l)
    {
        level = l;
    }
    
    public void SetPopulationSize(int index)
    {
        if (index == 0)
        {
            populationSize = 20;
        }
        else if (index == 1)
        {
            populationSize = 30;
        }
        else if (index == 2)
        {
            populationSize = 40;
        }
        else if (index == 3)
        {
            populationSize = 50;
        }
        else if (index == 4)
        {
            populationSize = 60;
        }
        else if (index == 5)
        {
            populationSize = 70;
        }
        else if (index == 6)
        {
            populationSize = 80;
        }
        else if (index == 7)
        {
            populationSize = 90;
        }
        else if (index == 8)
        {
            populationSize = 100;
        }
    }

    public void SetChromosomeLength(int index) {
        if (index == 0)
        {
            chromosomeLength = 20;
        }
        else if (index == 1)
        {
            chromosomeLength = 30;
        }
        else if (index == 2)
        {
            chromosomeLength = 40;
        }
        else if (index == 3)
        {
            chromosomeLength = 50;
        }
        else if (index == 4)
        {
            chromosomeLength = 60;
        }
        else if (index == 5)
        {
            chromosomeLength = 70;
        }
        else if (index == 6)
        {
            chromosomeLength = 80;
        }
        else if (index == 7)
        {
            chromosomeLength = 90;
        }
        else if (index == 8)
        {
            chromosomeLength = 100;
        }
    }
    public void SetEliteCount(int index)
    {
        if (index == 0)
        {
            eliteCount = 0;
        }
        else if (index == 1)
        {
            eliteCount = 2;
        }
        else if (index == 2)
        {
            eliteCount = 4;
        }
        else if (index == 3)
        {
            eliteCount = 6;
        }
        else if (index == 4)
        {
            eliteCount = 8;
        }
        else if (index == 5)
        {
            eliteCount = 10;
        }
    }

    public void SetSelection(int index)
    {
        if (index == 0)
        {
            selection = Selections.SelectionsOptions.RouletteWheelSelection;
        }
        else if (index == 1)
        {
            selection = Selections.SelectionsOptions.TournamentSelection;
        }
        else if (index == 2)
        {
            selection = Selections.SelectionsOptions.RandomTournamentSelection;
        }
        else if (index == 3)
        {
            selection = Selections.SelectionsOptions.StochasticTournamentSelection;
        }
        else if (index == 4)
        {
            selection = Selections.SelectionsOptions.TruncationSelection;
        }
        else if (index == 5)
        {
            selection = Selections.SelectionsOptions.RankSelection;
        }
        else if (index == 6)
        {
            selection = Selections.SelectionsOptions.SpecialRouletteWheelSelection;
        }
    }

    public void SetMutation(int index)
    {
        if (index == 0)
        {
            mutation = Mutations.MutationsOptions.UniformMutation;
        }
        else if (index == 1)
        {
            mutation = Mutations.MutationsOptions.AdaptativeMutation;
        }
        else if (index == 2)
        {
            mutation = Mutations.MutationsOptions.Swap1Mutation;
        }
        else if (index == 3)
        {
            mutation = Mutations.MutationsOptions.Swap2Mutation;
        }
        else if (index == 4)
        {
            mutation = Mutations.MutationsOptions.ShiftMutation;
        }
        else if (index == 5)
        {
            mutation = Mutations.MutationsOptions.InversionMutation;
        }
        else if (index == 6)
        {
            mutation = Mutations.MutationsOptions.InsertionMutation;
        }
    }

    public void SetCross(int index)
    {
        if (index == 0)
        {
            cross = Crosses.CrossesOptions.SinglePointCrossover;
        }
        else if (index == 1)
        {
            cross = Crosses.CrossesOptions.TwoPointCrossover;
        }
        else if (index == 2)
        {
            cross = Crosses.CrossesOptions.MultiPointCrossover;
        }
        else if (index == 3)
        {
            cross = Crosses.CrossesOptions.UniformCrossover;
        }
        else if (index == 4)
        {
            cross = Crosses.CrossesOptions.BestFitnessCrossover;
        }
    }
}