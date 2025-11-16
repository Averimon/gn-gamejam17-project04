using UnityEngine;

public class HighscoreHelper : MonoBehaviour
{
    void Start()
    {
        HighscoreManager.Instance.FindUIElements();
        HighscoreManager.Instance.LoadHighscore();
        HighscoreManager.Instance.ResetScore();
    }
}
