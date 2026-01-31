using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Rules")]
    public float timeLimit = 30f;
    public int targetScore = 3;

    [Header("UI (TMP)")]
    public TMP_Text scoreText;
    public TMP_Text timeText;

    [Header("Result UI")]
    public GameObject resultPanel;   // ★Panel
    public TMP_Text resultText;      // ★Panelの中のText

    float timeLeft;
    int score;
    bool ended;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        timeLeft = timeLimit;
        score = 0;
        ended = false;

        if (resultPanel != null) resultPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (ended) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            if (score >= targetScore) Win();
            else Lose("TIME UP");
        }

        UpdateUI();

        // テスト用：Rでリスタート
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void AddScore(int amount)
    {
        if (ended) return;

        score += amount;
        UpdateUI();

        if (score >= targetScore)
            Win();
    }

    public void Lose(string reason)
    {
        if (ended) return;
        ended = true;

        ShowResult($"GAME OVER\n{reason}\nSCORE: {score}");

        Time.timeScale = 0f;
    }

    public void Win()
    {
        if (ended) return;
        ended = true;

        ShowResult($"CLEAR!\nSCORE: {score}");

        Time.timeScale = 0f;
    }

    void ShowResult(string message)
    {
        if (resultPanel != null) resultPanel.SetActive(true);
        if (resultText != null) resultText.text = message;
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"SCORE: {score} / {targetScore}";
        if (timeText != null) timeText.text = $"TIME: {Mathf.CeilToInt(timeLeft)}";
    }

    // ★ボタンから呼べるよう public
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
