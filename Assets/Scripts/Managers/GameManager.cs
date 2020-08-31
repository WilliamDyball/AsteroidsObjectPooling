using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private int iScore;
    private int iLives;
    public int iSpawnedAsteroids = 0;

    private const string HIGHSCORE_PREF = "HIGHSCORE";
    private int iHighScore;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    public GameObject restartButton;

    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
        }
        else if (GameManager.instance != this)
        {
            Destroy(GameManager.instance.gameObject);
            GameManager.instance = this;
        }
    }

    private void Start()
    {
        //Get current highscore if in the player prefs.
        if (PlayerPrefs.HasKey(HIGHSCORE_PREF))
        {
            iHighScore = GetiPref(HIGHSCORE_PREF);
        }
        else
        {
            iHighScore = 0;
        }
        iScore = 0;
        iLives = 3;
        UpdateScore();
        UpdateLives();
        SpawnAsteroids();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void GameOver()
    {
        //Check if iScore > Highscore if so update it.
        if (iScore > iHighScore)
        {
            iHighScore = iScore;
            SetPref(HIGHSCORE_PREF, iHighScore);
            //TODO display high score and current score.
        }
        restartButton.SetActive(true);
    }

    public void GainALife()
    {
        iLives++;
        UpdateLives();
    }

    public void PlayerDeath()
    {
        iLives--;
        if (iLives <= 0)
        {
            GameOver();
        }
        UpdateLives();
    }

    private void UpdateLives()
    {
        livesText.text = "Lives: " + iLives;
    }

    public void IncrementScore()
    {
        iScore++;
        UpdateScore();
        iSpawnedAsteroids--;
        if (iSpawnedAsteroids <= 0)
        {
            SpawnAsteroids();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + iScore;
    }

    private void SetPref(string _strKey, int _iValue)
    {
        PlayerPrefs.SetInt(_strKey, _iValue);
    }

    private int GetiPref(string _strKey, int _iDefault = 0)
    {
        return PlayerPrefs.GetInt(_strKey, Convert.ToInt32(_iDefault));
    }

    //Asteroid spawning
    public void SpawnAsteroids()
    {
        if (PoolsManager.instance != null)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 v3SpawnPos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), 0, Random.Range(0, Screen.height)));
                v3SpawnPos.y = 0;
                PoolsManager.instance.SpawnObjFromPool("AsteroidBase", v3SpawnPos, Quaternion.identity);
            }
        }
    }
}
