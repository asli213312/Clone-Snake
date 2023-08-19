using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Restart : MonoBehaviour
{
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private GameObject _mainPanel;
    [Inject] private SnakeFactory _snakeFactory;
    private ISnakeCollisionHandler _snakeCollisionHandler;
    private Menu _menu;
    private bool _isInitialized;

    private void Initialize()
    {
        if (_isInitialized)
        {
            _snakeCollisionHandler = _snakeFactory.GetCollisionHandler();
            _menu = GetComponent<Menu>();

            if (_snakeCollisionHandler != null)
            {
                _snakeCollisionHandler.SnakeCrash += OpenFailPanel;
                Debug.Log("Event in Restart is Subscribed!");
            }
            else
                Debug.LogError("CollisionHandler is Null in Restart");    
        }

        Debug.Log("Initialize in Restart is successful!");
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            _isInitialized = true;
            Initialize();
        }
    }
    
    private void OnDisable()
    {
        if (_snakeCollisionHandler != null)
        {
            _snakeCollisionHandler.SnakeCrash -= OpenFailPanel;
            Debug.Log("Event in Restart is UNsubscribed!");
        }
    }

    private void OpenFailPanel()
    {
        _menu.ClosePanel(_mainPanel);
        _menu.OpenPanel(_failPanel);
    }

    public void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
