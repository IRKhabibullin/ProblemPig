using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;

    public void OnScoreChanged()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }

}
