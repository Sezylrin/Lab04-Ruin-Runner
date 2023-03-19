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
    public int level { get; private set; } = 1;
    private int coinsCollected = 0;
    private bool canSpawnKey = true;
    private Vector2[] keySpawnLocations = new Vector2[] { 
        new Vector2(-9.6f, -13.7f),
        new Vector2(-46.5f, -13.7f),
    };

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
        lives = 3;
        score = 0;
        coinsCollected = 0;
    }

    public void CollectKey()
    {
        ++keysCollected;
        LevelManager.Instance.CollectKey(keysCollected - 1);
    }

    public void DecrementLives()
    {
        if (lives == 0)
        {
            Loader.Load(Scene.GameOver);
        }
        else
        {
            lives--;
            LevelManager.Instance.SetLives(lives.ToString());
        }
    }

    public void CollectCoin(Vector3 playerPosition)
    {
        coinsCollected++;
        AddToScore(50);
        LevelManager.Instance.SetScore(score.ToString());
        if (coinsCollected % coinAmountToSpawnKey == 0 && canSpawnKey)
        {
            SpawnKey(playerPosition);
        }
    }

    private void SpawnKey(Vector3 playerPosition)
    {
        if (keysCollected == level)
        {
            canSpawnKey = false;
        }
        Vector2 spawnLocation = FindFurthestSpawnLocation(playerPosition);
        Instantiate(key, spawnLocation, Quaternion.identity);
    }

    private void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        LevelManager.Instance.SetScore(score.ToString());
    }

    private Vector2 FindFurthestSpawnLocation(Vector3 playerPosition)
    {
        float maxDistance = 0f;
        Vector2 furthestSpawnLocation = Vector2.zero;

        foreach (Vector2 spawnLocation in keySpawnLocations)
        {
            float distance = Vector2.Distance(playerPosition, spawnLocation);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestSpawnLocation = spawnLocation;
            }
        }

        return furthestSpawnLocation;
    }
    
    public bool CanExitLevel()
    {
        return level == keysCollected;
    }
}
