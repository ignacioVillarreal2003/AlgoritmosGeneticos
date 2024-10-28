using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelReset : MonoBehaviour
{
    public Button myButton;
    void Start()
    {
        LevelController levelController = FindObjectOfType<LevelController>();
        
        if (myButton != null && levelController != null)
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(() => levelController.Reset());
        }
    }
}
