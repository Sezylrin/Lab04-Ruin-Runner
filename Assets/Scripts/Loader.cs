using UnityEngine.SceneManagement;
public enum Scene
{
    MainMenu,
    Level1,
    GameOver,
    Victory,
}

public static class Loader
{
    public static void Load(Scene scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
    }
}
