using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool levelEnd, gameOver;

    Counter counter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Start()
    {
        counter = gameObject.AddComponent<Counter>();
    }

    public void LevelEnd()
    {
        int buildings = counter.CountRemainingBuildings();
        int ammo = counter.CountRemainingAmmunition();

        if (!levelEnd && !gameOver)
        {
            StartCoroutine(ScoreManager.Instance.AnimatedScoreEnumerator(buildings, ammo));
            levelEnd = true;
        }
    }

    public void GameOver()
    {
        UIManager.Instance.GameOverSwitch(true);
        gameOver = true;
        Time.timeScale = 0;
    }
}
