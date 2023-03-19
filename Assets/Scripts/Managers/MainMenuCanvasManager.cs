using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvasManager : MonoBehaviour
{
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button quitGameBtn;

    private Scene sceneToLoad = Scene.Level1;
    void Start()
    {
        startGameBtn.onClick.AddListener(delegate () { StartGame(); });
        quitGameBtn.onClick.AddListener(delegate () { QuitGame(); });
    }

    public void StartGame()
    {
        Loader.Load(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetSceneToLoad(Scene scene)
    {
        sceneToLoad = scene;
    }
}
