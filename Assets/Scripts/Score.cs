using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public static Score Instance { get; private set; }
    private double score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void UpdateScore(double addedValue)
    {
        score += addedValue;
        UpdateScoreVisual();
    }
    private void UpdateScoreVisual()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}