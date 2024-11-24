using UnityEngine;
using TMPro;

public class ActualDistance : MonoBehaviour
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
        textMesh.text = "Distancia actual " + geneticController.GetActualDistance().ToString("F4");
    }
}