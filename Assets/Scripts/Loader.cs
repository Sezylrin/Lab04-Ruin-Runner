using UnityEngine.SceneManagement;
public enum Scene
{
    MainMenu,
    GameOver,
    Victory,
    Level1,
}

public static class Loader
{
    public static void Load(Scene scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
    }
}
