using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Singleton GameManager controlling score, win/loss states, and UI screens.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Win Conditions")]
    [SerializeField] private int scrapToWin = 10; // Set required target scrap count

    [Header("UI Text References")]
    [SerializeField] private TMP_Text scoreText;

    [Header("UI Panel References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;

    public int CurrentScore { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CurrentScore = 0;
        IsGameOver = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameWinPanel != null) gameWinPanel.SetActive(false);

        UpdateScoreUI();
    }

    /// <summary>
    /// Call to add score when scrap is collected. Checks win condition.
    /// </summary>
    public void AddScore(int amount)
    {
        if (IsGameOver) return;

        CurrentScore += amount;
        UpdateScoreUI();

        // Check if player reached the winning threshold
        if (CurrentScore >= scrapToWin)
        {
            GameWin();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Scrap: {CurrentScore} / {scrapToWin}";
        }
    }

    /// <summary>
    /// Triggers the Win state and opens the Win Screen.
    /// </summary>
    public void GameWin()
    {
        IsGameOver = true;
        Debug.Log("Player won the game by collecting all required scrap!");

        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
        }

        // Freeze gameplay
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Triggers the Game Over loss state.
    /// </summary>
    public void GameOver(bool won = false)
    {
        if (won)
        {
            GameWin();
            return;
        }

        IsGameOver = true;
        Debug.Log("Game Over - Player destroyed!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    // UI Button Methods
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Make sure scene name matches Build Settings
    }
}