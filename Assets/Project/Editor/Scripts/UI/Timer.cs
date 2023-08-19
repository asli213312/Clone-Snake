using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _timer;

    [Inject] private SnakeFactory _snakeFactory;
    private ISnakeMovement _moverSnake;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        _timerText.text = FormatTimer(_timer);

        CheckSnakeIsTooSmall();
    }

    private void CheckSnakeIsTooSmall()
    {
        ISnakeMovement moverSnake = _snakeFactory.GetMover();
        List<Body> bodiesSnake = moverSnake.GetBodyParts();
    }
    
    private string FormatTimer(float seconds)
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
    }
}
