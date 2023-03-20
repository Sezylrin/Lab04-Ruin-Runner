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
    private int keySpawned = 0;
    private bool canSpawnKey = true;

    public List<List<Vector2>> keySpawnLocations = new List<List<Vector2>>(); 

    private void Awake()
    {
        List<Vector2> levelOne = new List<Vector2>();
        levelOne.Add(new Vector2(-5f, -11.5f));
        levelOne.Add(new Vector2(-42f, -11.5f));
        levelOne.Add(new Vector2(-33f, 10.5f));
        keySpawnLocations.Add(levelOne);
        List<Vector2> levelTwo = new List<Vector2>();
        levelTwo.Add(new Vector2(-5f, 10.5f));
        levelTwo.Add(new Vector2(-5f, -5.5f));
        levelTwo.Add(new Vector2(-39f, -8.5f));
        levelTwo.Add(new Vector2(-42f, 10.5f));
        keySpawnLocations.Add(levelTwo);
        List<Vector2> levelThree = new List<Vector2>();
        levelThree.Add(new Vector2(-38f, 10.5f));
        levelThree.Add(new Vector2(-42f, -6.5f));
        levelThree.Add(new Vector2(-20f, -8.5f));
        levelThree.Add(new Vector2(-5f, 10.5f));
        keySpawnLocations.Add(levelThree);
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
        keySpawned = 0;
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
        if (keySpawned == level)
        {
            canSpawnKey = false;
        }
        if (!canSpawnKey) return;
        keySpawned++;
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

        foreach (Vector2 spawnLocation in keySpawnLocations[level - 1])
        {
            float distance = Vector2.Distance(playerPosition, spawnLocation);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestSpawnLocation = spawnLocation;
            }
        }
        keySpawnLocations[level - 1].Remove(furthestSpawnLocation);
        return furthestSpawnLocation;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void IncrementLevel()
    {
        level++;
    }
    
    public bool CanExitLevel()
    {
        return level == keysCollected;
    }
}
