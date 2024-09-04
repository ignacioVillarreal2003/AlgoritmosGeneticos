using UnityEngine;
using TMPro;

public class DataUI : MonoBehaviour
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
        textMesh.text = "Poblacion total de " + geneticController.getPopulationSize().ToString() + ". Genes por individuo " +  geneticController.getAdnLength().ToString();
    }
}
