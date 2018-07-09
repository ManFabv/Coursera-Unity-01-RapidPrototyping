using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class HUD : MonoBehaviour
{
    public enum Language
    {
        English, Spanish, French
    }

    public Language language;

    public Text scoreText;
    private string scoreString = "Scored: ";
    public Text livesText;
    private string livesString = "Life: ";
    public Text enemyText;
    private string enemyString = "Foes: ";

    public Canvas canvas;

    private void Awake()
    {
        if (scoreText != null)
            scoreText.text = string.Empty;

        if(livesText != null)
            livesText.text = string.Empty;

        if(enemyText != null)
            enemyText.text = string.Empty;

        if(canvas == null)
            canvas = GameObject.FindObjectOfType<Canvas>();
    }

    private void UpdateText(Text textObj, string words, bool value)
    {
        if(textObj != null)
            textObj.text = Concatenate(words, value);
    }

    private string Concatenate(string words, bool value)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(words);
        builder.Append(value);

        return builder.ToString();
    }

    private void UpdateText(Text textObj, string words, float value)
    {
        if (textObj != null)
            textObj.text = Concatenate(words, value);
    }

    private string Concatenate(string words, float value)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(words);
        builder.Append(value);

        return builder.ToString();
    }

    public void UpdateLives(int lives)
    {
        UpdateText(livesText, livesString, lives);
    }

    public void UpdateScore(int score)
    {
        UpdateText(scoreText, scoreString, score);
    }

    public void UpdateEnemies(int enemies)
    {
        UpdateText(enemyText, enemyString, enemies);
    }

    private void UpdateText(Text textObj, string words, int value)
    {
        if (textObj != null)
            textObj.text = Concatenate(words, value);
    }

    private string Concatenate(string words, int value)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(words);
        builder.Append(value);

        return builder.ToString();
    }

    public void GameOver()
    {
        if(canvas != null)
        {
            LoaderManager.Instance.StartLoader();
            
            GameObject.Destroy(canvas.gameObject);
        }
    }
}