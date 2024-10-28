using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    public Button myButton;
    [SerializeField] private int level = 3;

    void Start()
    {
        LevelController levelController = FindObjectOfType<LevelController>();
        
        if (myButton != null && levelController != null)
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(() => levelController.SetLevel(level));
        }
    }
}
