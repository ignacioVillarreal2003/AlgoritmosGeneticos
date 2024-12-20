using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void LoadScene(int n)
    {
        SceneManager.LoadScene(n);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayScene()
    {
        SceneManager.LoadScene(LevelController.Instance.GetLevel());
    }
}
