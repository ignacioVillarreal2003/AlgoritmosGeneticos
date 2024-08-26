using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InitializeMovements : MonoBehaviour
{
    private List<(int, int, int, int)> movements = new List<(int, int, int, int)>();
    private int numberOfMovements = 100;

    void Start()
    {
        GenerateMovements();
    }

    void GenerateMovements()
    {
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
    }

    public int GetNumberOfMovements()
    {
        return numberOfMovements;
    }

    public List<(int, int, int, int)> GetMovements()
    {
        return movements;
    }
}
