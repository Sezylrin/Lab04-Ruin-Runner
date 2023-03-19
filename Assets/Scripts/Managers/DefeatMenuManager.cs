using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatMenuManager : MonoBehaviour
{
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button mainMenuBtn;

    void Start()
    {
        retryBtn.onClick.AddListener(delegate () { Retry(); });
        mainMenuBtn.onClick.AddListener(delegate () { MainMenu(); });
    }

    public void Retry()
    {
        Loader.Load(Scene.Level1);
    }

    public void MainMenu()
    {
        Loader.Load(Scene.MainMenu);
    }
}
