using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public float totalTimeInSeconds = 300.0f;
    private float currentTime;
    private bool isRunning = true;
    public event Action<int, int> OnTimerTick; // (minutes, seconds)
    public event Action OnTimerEnd;
    private float tickTimer = 1.0f;

    public TextMeshProUGUI timerText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentTime = totalTimeInSeconds;
    }

    private void Update()
    {
        if (!isRunning) { return; }

        currentTime -= Time.deltaTime;
        tickTimer -= Time.deltaTime;

        if (tickTimer <= 0)
        {
            tickTimer = 1.0f;
            int minutes = Mathf.FloorToInt(currentTime / 60.0f);
            int seconds = Mathf.FloorToInt(currentTime % 60.0f);
            OnTimerTick?.Invoke(minutes, seconds);
            UpdateTimerUI(minutes, seconds);
        }

        if (currentTime <= 0.0f)
        {
            isRunning = false;
            currentTime = 0.0f;
            OnTimerEnd?.Invoke();
            GameInstance.Instance.gameEnd = true;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    private void UpdateTimerUI(int minutes, int seconds)
    {
        if (timerText != null)
        {
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }

    public void SetCurrentTimer(float seconds)
    {
        currentTime = seconds;
        int minutes = Mathf.FloorToInt(currentTime / 60.0f);
        int secondsLeft = Mathf.FloorToInt(currentTime % 60.0f);
        UpdateTimerUI(minutes, secondsLeft);
    }
}
