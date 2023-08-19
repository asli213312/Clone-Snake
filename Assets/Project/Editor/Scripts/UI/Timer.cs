using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private ISnakeMovement _moverSnake;

    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;

        _timerText.text = FormatTimer(_timer);
    }

    private string FormatTimer(float seconds)
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
    }
}
