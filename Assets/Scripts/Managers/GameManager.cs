using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int coinAmountToSpawnKey;
    [SerializeField] private GameObject key;

    public int keysCollected { get; private set; }
    public int lives { get; private set; }
    public int score { get; private set; }
    private int coinsCollected = 0;
    private int level = 1;
    private bool canSpawnKey = true;
    private Vector2[] keySpawnLocations = new Vector2[] { new Vector2(-10f, 7.55f) };

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
        coinsCollected = 0;
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

    public void CollectKey()
    {
        ++keysCollected;
        LevelManager.Instance.CollectKey(keysCollected - 1);
    }

    public void DecrementLives()
    {
        lives--;
        LevelManager.Instance.SetLives(lives.ToString());
    }

    public void CollectCoin()
    {
        coinsCollected++;
        AddToScore(50);
        LevelManager.Instance.SetScore(score.ToString());
        if (coinsCollected % coinAmountToSpawnKey == 0 && canSpawnKey)
        {
            SpawnKey();
        }
    }

    private void SpawnKey()
    {
        if (keysCollected == level)
        {
            canSpawnKey = false;
        }
        Instantiate(key, keySpawnLocations[0], Quaternion.identity);
    }

    private void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        LevelManager.Instance.SetScore(score.ToString());
    }
}
