using UnityEngine;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highscoreText;

    private int _score;

    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            UpdateUI();
        }
    }

    private void Awake()
    {
        FindUIElements();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadHighscore();
            UpdateUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FindUIElements()
    {
        _scoreText = GameObject.FindGameObjectWithTag("Score")?.GetComponent<TMP_Text>();
        _highscoreText = GameObject.FindGameObjectWithTag("Highscore")?.GetComponent<TMP_Text>();
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void SubtractScore(int amount)
    {
        Score -= amount;
        if (Score < 0)
            Score = 0;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void SaveHighscore()
    {
        if (Score > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", Score);
            PlayerPrefs.Save();
        }
    }

    public void LoadHighscore()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        if (_highscoreText != null)
            _highscoreText.text = " Highscore: " + highscore.ToString();
    }

    private void UpdateUI()
    {
        if (_scoreText != null)
            _scoreText.text = " Score: " + Score.ToString();
    }
}