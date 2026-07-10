using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static int savedScore = 0;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    [Header("Game Stats")]
    public int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            score = savedScore;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        savedScore = score;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public static void ResetStoredData()
    {
        savedScore = 0;
    }
}