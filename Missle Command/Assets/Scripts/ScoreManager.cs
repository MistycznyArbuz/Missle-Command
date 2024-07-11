using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [Header("UI")]
    [Tooltip("UI Elements")]
    public TMP_Text scoreText, highscoreText, levelEndScore;
    [Tooltip("UI Elements")]
    public GameObject bonusScore;
    [Header("Audio")]
    [Tooltip("Audio source of score")]
    public AudioSource scoreAudio;
    public AudioClip scoreUp, highscoreUp;

    [SerializeField]
    private int score;
    public int Score { get { return score; } set { score = value; } }

    private int HighScore;
    public int highScore { get { return LoadHighscore(); } set { SaveHighscore(value); } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void SaveHighscore(int value)
    {
        HighScore = value;
        PlayerPrefs.SetInt("highscore", HighScore);
    }

    public int LoadHighscore()
    {
        return HighScore = PlayerPrefs.GetInt("highscore");
    }

    public void Update()
    {
        scoreText.text = "Score : " + score;
        levelEndScore.text = "Score : " + score;
        highscoreText.text = "Highscore : " + HighScore;
    }

    public void CreateBonus(string text)
    {
        GameObject bonus = Instantiate(bonusScore, transform);
        bonus.GetComponent<TextMesh>().text = text;
        bonus.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.5f, 0.5f), 1), ForceMode2D.Impulse);
    }

    public IEnumerator AnimatedScoreEnumerator(int buildings, int ammo)
    {
        LoadHighscore();
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < buildings; i++)
        {
            yield return new WaitForSeconds(0.2f);
            scoreAudio.PlayOneShot(scoreUp);
            CreateBonus("x100");
            score += 100;
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < ammo; i++)
        {
            yield return new WaitForSeconds(0.075f);
            scoreAudio.PlayOneShot(scoreUp);
            CreateBonus("x5");
            score += 5;
        }
        yield return new WaitForSeconds(2.5f);
        if (score > highScore)
        {
            scoreAudio.PlayOneShot(highscoreUp);
            HighScore = Score;
            SaveHighscore(HighScore);
        }
        yield return new WaitForSeconds(2.5f);
        if (!GameManager.Instance.gameOver)
            LevelGenerator.Instance.NextLevel();
        yield break;
    }
}
