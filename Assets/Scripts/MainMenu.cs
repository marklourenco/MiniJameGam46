using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        int highscore = HighscoreManager.LoadHighscore();
        highScoreText.text = "Highscore: " + highscore.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}