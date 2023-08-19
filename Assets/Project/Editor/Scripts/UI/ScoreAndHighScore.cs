using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreAndHighScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private PersistentData persistentData;

    [Inject] private FoodFactory _foodFactory;
    private PoolFood _poolFood;
    
    private int _score;

    public void Initialize()
    {
        _poolFood = _foodFactory.GetPool();
        if (_poolFood != null)
        {
            Debug.Log("Pool is initialized in ScoreAndHighScore");
            _poolFood.FoodScoreChanged += HandleFoodScoreChanged;
        }
        
        if (persistentData != null)
            UpdateHighScore();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        if (_poolFood != null)
        {
            _poolFood.FoodScoreChanged -= HandleFoodScoreChanged;
        }
    }

    private void HandleFoodScoreChanged(int newScore)
    {
        _score += newScore;
        scoreText.text = _score.ToString();
        
        CheckHighScore(_score);
        UpdateHighScore();
    }
    
    private void UpdateHighScore()
    {
        highScoreText.text = persistentData.highScore.ToString();
    }
    
    public void CheckHighScore(int newScore)
    {
        if (newScore > persistentData.highScore)
        {
            persistentData.highScore = newScore;
            SaveHighScore();
        }
    }
    
    private void SaveHighScore()
    {
        PlayerPrefs.SetString("HighScore", JsonUtility.ToJson(persistentData));
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        string highScoreJson = PlayerPrefs.GetString("HighScore");
        if (!string.IsNullOrEmpty(highScoreJson))
        {
            persistentData = JsonUtility.FromJson<PersistentData>(highScoreJson);
        }
    }
}
