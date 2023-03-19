using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCanvasManager : MonoBehaviour
{
    [SerializeField] private Image[] keys;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text lives;

    public void NewGame()
    {
        keys[0].GetComponent<Image>().enabled = false;
        keys[1].GetComponent<Image>().enabled = false;
        keys[2].GetComponent<Image>().enabled = false;
        score.text = "0";
        lives.text = "3";
    }

    public void SetLives(string newLives)
    {
        lives.text = newLives;
    }

    public void SetScore(string newScore)
    {
        score.text = newScore;
    }

    public void CollectKey(int keyToSpawn)
    {
        keys[keyToSpawn].GetComponent<Image>().enabled = true;
    }
}
