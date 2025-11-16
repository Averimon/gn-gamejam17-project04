using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // ⬅ wichtig fürs neue Input System

public class BackToMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;

    private bool _isPaused = false;

    private void Awake()
    {
        menuCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Ensure normal time on game start
    }

    private void Update()
    {
        // Neues Input System: Tastatur abfragen
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        menuCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    private void Resume()
    {
        menuCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void ClickYes()
    {
        Time.timeScale = 1f;
        HighscoreManager.Instance.SaveHighscore();
        SceneManager.LoadScene("MainMenu");
    }

    public void ClickNo()
    {
        Resume();
    }
}