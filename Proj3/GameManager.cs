using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text element for displaying the score
    public TextMeshProUGUI highScoreText; // Reference to the UI Text element for displaying the high score

    private int currentScore = 0;
    private int highScore = 0;


    public static GameManager Instance { get; private set; }

    // Add other GameManager variables and methods as needed

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager between scenes if needed
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }


    void Start()
    {
        // Load the high score from PlayerPrefs when the game starts
        LoadHighScore();
        UpdateUI();
    }

    void UpdateUI()
    {
        // Update the text UI elements with current score and high score values
        scoreText.text = "Score: " + currentScore.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();

    }

    public void IncreaseScore(int points)
    {
        // Increase the current score by the given points
        currentScore += points;

        // Check if the current score beats the high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore(); // Save the new high score
        }

        UpdateUI(); // Update the UI to reflect the changes in score
        
    }

    void SaveHighScore()
    {
        // Save the high score to PlayerPrefs
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    void LoadHighScore()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void ResetScore()
    {
        // Reset current score to zero
        currentScore = 0;
        UpdateUI(); // Update the UI to reflect the reset score
    }

    public void DeactivateGameManager()
    {
        gameObject.SetActive(false);
    }

    public void ActivateGameManager()
    {
        gameObject.SetActive(true);
    }

        public void QuitGame()
    {
        Destroy(PlayerController.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex * 0);
        Destroy(gameObject);
    }


}
