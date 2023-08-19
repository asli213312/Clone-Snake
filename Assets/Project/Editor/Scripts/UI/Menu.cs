using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 0;
    }
    
    public void OpenPanel(GameObject panel) 
    {
        panel.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void ClosePanel(GameObject panel) 
    {
        panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
