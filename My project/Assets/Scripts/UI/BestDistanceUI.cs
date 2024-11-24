using UnityEngine;
using TMPro;

public class BestFitnessUI : MonoBehaviour
{
    private GeneticController geneticController;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        geneticController = FindAnyObjectByType<GeneticController>();
    }

    void Update()
    {
        textMesh.text = "Mejor distancia " + geneticController.GetBestDistance().ToString("F4");
    }
}
