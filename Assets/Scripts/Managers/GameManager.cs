using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int keysCollected { get; private set; }
    public int lives { get; private set; }
    public int score { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void NewGame()
    {
        keysCollected = 0;
        lives = 0;
        score = 0;
    }

    public void Respawn()
    {
        if (--lives == 0)
        {
            //GameOver
        }
        else
        {
            //Respawn
        }
    }

    public void IncrementKeysCollected(int keysToAdd = 1)
    {
        keysCollected += keysToAdd;
    }

    public void DecrementLives()
    {
        lives--;
    }

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
}
