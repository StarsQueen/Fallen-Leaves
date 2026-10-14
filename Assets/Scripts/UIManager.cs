using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    [Header("Health (paws, left to right)")]
    [SerializeField] Image[] paws;
    [SerializeField] Sprite pawFull;   // pink paw
    [SerializeField] Sprite pawEmpty;  // grey paw

    [SerializeField] GameObject gameOverPanel;

    void OnEnable()
    {
        GameEvents.OnScoreChanged += UpdateScore;
        GameEvents.OnHealthChanged += UpdateHealth;
        GameEvents.OnGameOver += ShowGameOver;
    }

    void OnDisable()
    {
        GameEvents.OnScoreChanged -= UpdateScore;
        GameEvents.OnHealthChanged -= UpdateHealth;
        GameEvents.OnGameOver -= ShowGameOver;
    }

    void Start() => UpdateScore(0);

    void UpdateScore(int s) => scoreText.text = $"Score: {s}";

    void UpdateHealth(int h)
    {
        for (int i = 0; i < paws.Length; i++)
            paws[i].sprite = i < h ? pawFull : pawEmpty;
    }

    void ShowGameOver() => gameOverPanel.SetActive(true);
}