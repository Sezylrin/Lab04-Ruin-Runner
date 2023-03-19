using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelCanvasManager levelCanvasManager;

    public static LevelManager Instance { get; private set; }
    void Awake()
    {
        levelCanvasManager.NewGame();
        GameManager.Instance.NewGame();
        Instance = this;
    }

    public void SetLives(string newLives)
    {
        levelCanvasManager.SetLives(newLives);
    }

    public void SetScore(string newScore)
    {
        levelCanvasManager.SetScore(newScore);
    }

    public void CollectKey(int keyToSpawn)
    {
        levelCanvasManager.CollectKey(keyToSpawn);
    }
}
